using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public static GridSystem current;

    public GridLayout gridLayout;
    Grid grid;
    [SerializeField] Tilemap mainTilemap;
    [SerializeField] TileBase whiteTile;

    public GameObject prefab1;
    public GameObject prefab2;

    PlaceableObject objectToPlace;

    private void Awake()
    {
        current = this;

        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    public static Vector3 GetTouchWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    public void InitializeWithObject(GameObject prefab){
        Vector3 position = SnapCordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }
}
