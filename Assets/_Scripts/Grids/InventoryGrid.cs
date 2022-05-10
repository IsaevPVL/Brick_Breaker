using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//[ExecuteInEditMode]
public class InventoryGrid : MonoBehaviour
{
    public static event Action<Vector2> NewScale;
    public static event Action<PlaceableObject> ObjectPlaced;

    public static InventoryGrid active;
    public Transform element;

    Vector3Int cellPosition;
    public Boundaries boundaries;
    Vector3 bottomLeftCorner;

    [Header("Inventory Grid")]
    public Vector2Int inventoryDimensions;
    public GridLayout inventoryGridLayout;
    [Range(0, 0.2f)] public float inventoryHorizontalPadding;
    [Range(0, 0.2f)] public float inventoryVerticalPadding;

    public Vector3 inventoryTopRightCorner;
    public Vector3 inventoryBottomLeftCorner;
    public float horizontalSector;
    public float verticalSector;
    public Vector2 inventoryPadding;

    public Dictionary<Vector3Int, PlaceableObject> cellFree;
    Vector3Int startCell;

    private void Awake()
    {
        if (active != null && active != this)
        {
            Destroy(this);
        }
        else
        {
            active = this;
        }
    }

    private void Start()
    {
        CalculateGrid();
        InitializeCellAvailability();
    }

    public void InitializeCellAvailability()
    {
        cellFree = new Dictionary<Vector3Int, PlaceableObject>(inventoryDimensions.x * inventoryDimensions.y);

        Vector3Int current = Vector3Int.zero;
        for (int y = 0; y < inventoryDimensions.y; y++)
        {
            current.y = y;
            for (int x = 0; x < inventoryDimensions.x; x++)
            {
                current.x = x;
                cellFree[current] = null;
            }
        }
    }

    public bool PlaceObject(PlaceableObject obj, bool toCell = false)
    {
        if (toCell)
        {
            startCell = obj.defaultCell;
        }
        else
        {
            startCell = ToCell(obj.touchPosition + obj.objectTouchOffset);
            if (startCell == ToCell(obj.transform.position) || !cellFree.ContainsKey(startCell) || !cellFree.ContainsKey(startCell + new Vector3Int(obj.dimensions.x - 1, obj.dimensions.y - 1, 0))) return false;

            foreach (Vector3Int cell in obj.occupiedCells)
            {
                cellFree[cell] = null;
            }
        }

        bool isFree = true;
        Vector3Int currentCell = Vector3Int.zero;
        Stack<Vector3Int> tempCells = new Stack<Vector3Int>(obj.dimensions.x * obj.dimensions.y);

        for (int y = startCell.y; y < startCell.y + obj.dimensions.y; y++)
        {
            currentCell.y = y;
            for (int x = startCell.x; x < startCell.x + obj.dimensions.x; x++)
            {
                currentCell.x = x;
                if (cellFree[currentCell] != null)
                {
                    isFree = false;
                }
                else
                {
                    tempCells.Push(currentCell);
                }
            }
        }

        if (isFree)
        {
            //print(obj.occupiedCells.Count);
            obj.occupiedCells.Clear();
            for (int i = tempCells.Count; i > 0; i--)
            {
                obj.occupiedCells.Push(tempCells.Peek());
                cellFree[tempCells.Pop()] = obj;
            }
            obj.transform.position = inventoryGridLayout.CellToWorld(startCell) + new Vector3(inventoryGridLayout.cellGap.x / 2, 0, 0);

            if (!toCell)
            {
                ObjectPlaced?.Invoke(obj);
            }
            return true;
        }
        else
        {
            if (toCell)
            {
                return false;
            }
            else
            {
                foreach (Vector3Int cell in obj.occupiedCells)
                {
                    cellFree[cell] = obj;
                }

                return false;
            }
        }
    }

    Vector3Int ToCell(Vector3 position)
    {
        return inventoryGridLayout.WorldToCell(position);
    }

    public void CalculateGrid()
    {
        Grid inventoryGrid = inventoryGridLayout.GetComponent<Grid>();
        //Grid fieldGrid = fieldGridLayout.GetComponent<Grid>();

        boundaries = GameObject.FindObjectOfType<Boundaries>();
        bottomLeftCorner = boundaries.corners[3];
        Vector3 bottomBoundCentre = new Vector3(0, bottomLeftCorner.y, 0);
        Vector3 closestPaddlePoint = GameObject.FindGameObjectWithTag("Paddle").GetComponent<Rigidbody>().ClosestPointOnBounds(bottomBoundCentre);

        float inventoryWidth = Mathf.Abs(bottomLeftCorner.x) * 2 - boundaries.lineWidth;
        float inventoryHeight = Mathf.Abs(bottomLeftCorner.y - closestPaddlePoint.y);

        inventoryPadding = new Vector2();
        inventoryPadding.x = inventoryWidth * inventoryHorizontalPadding;
        inventoryPadding.y = inventoryHeight * inventoryVerticalPadding;


        horizontalSector = (inventoryWidth - inventoryPadding.x * 2) / inventoryDimensions.x;
        verticalSector = (inventoryHeight - inventoryPadding.y * 2) / inventoryDimensions.y;

        //SCALING TEST
        //element.localScale = new Vector3(horizontalSector, verticalSector, 0.2f);
        NewScale?.Invoke(new Vector2(horizontalSector, verticalSector));

        inventoryGrid.cellSize = new Vector3(horizontalSector - inventoryGrid.cellGap.x, verticalSector - inventoryGrid.cellGap.y, 0);
        transform.position = boundaries.corners[3] + new Vector3(inventoryGrid.cellGap.x + inventoryPadding.x, (inventoryGrid.cellGap.y / 2) + inventoryPadding.y, 0.3f);

        inventoryBottomLeftCorner = transform.position;
        //inventoryBottomLeftCorner.x = inventoryBottomLeftCorner.x - inventoryGrid.cellGap.x;
        //inventoryBottomLeftCorner.y = inventoryBottomLeftCorner.y - inventoryGrid.cellGap.y;
        inventoryTopRightCorner = inventoryGridLayout.CellToWorld(new Vector3Int(inventoryDimensions.x, inventoryDimensions.y, 0));
    }

    public Vector3 SnapCordinateToGrid()
    {
        Vector3 position = inventoryGridLayout.CellToWorld(cellPosition) + new Vector3(inventoryGridLayout.cellGap.x / 2, 0, 0);
        return position;

        //position = inventoryGrid.GetCellCenterWorld(cellPosition) + new Vector3((inventoryGrid.cellSize.x ) / 2, -inventoryGrid.cellGap.y / 2, 0);
    }

    public bool WithinGridBoundaries(Vector3 position)
    {
        cellPosition = inventoryGridLayout.WorldToCell(position);
        //Debug.Log("Cell: " + cellPosition);
        return (cellPosition.x < inventoryDimensions.x && cellPosition.x >= 0) && (cellPosition.y < inventoryDimensions.y && cellPosition.y >= 0);
    }
}
