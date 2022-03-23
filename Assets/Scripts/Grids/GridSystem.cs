using UnityEngine;
using System;

public class GridSystem : MonoBehaviour
{   
    public static event Action<Vector2> NewScale;

    public static GridSystem active;
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

    [Space]
    [Header("Field Grid")]
    public Vector2 fieldDimensions;
    public GridLayout fieldGridLayout;

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

        boundaries = GameObject.FindObjectOfType<Boundaries>();
    }

    private void Start()
    {
        Grid inventoryGrid = inventoryGridLayout.GetComponent<Grid>();
        //Grid fieldGrid = fieldGridLayout.GetComponent<Grid>();

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

    // Vector2 FindInventoryPadding(){

    //     return padding;
    // }
}
