using UnityEngine;
using System.Collections;

public class InventoryBorders : MonoBehaviour
{
    public float lineWidth = 0.05f;
    public Material lineMaterial;

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.1f); //CHANGE!!!?

        InventoryGrid grid = InventoryGrid.active;

        Vector3 bottomLeftCorner = grid.inventoryBottomLeftCorner;
        Vector3 topRightCorner = grid.inventoryTopRightCorner;

        Vector2Int gridDimensions = grid.inventoryDimensions;

        float horizontalCell = grid.inventoryGridLayout.cellSize.x + grid.inventoryGridLayout.cellGap.x;
        float verticalCell = grid.inventoryGridLayout.cellSize.y + grid.inventoryGridLayout.cellGap.y;

        for (int i = 0; i <=gridDimensions.x; i++)
        {
            Vector3 verticalLineBottom = new Vector3();
            verticalLineBottom.x = bottomLeftCorner.x + (horizontalCell * i);
            verticalLineBottom.y = bottomLeftCorner.y;
            verticalLineBottom.z = bottomLeftCorner.z;
            NewLine(verticalLineBottom, new Vector3(verticalLineBottom.x, topRightCorner.y, topRightCorner.z));
        }

        for (int i = 0; i <= gridDimensions.y; i++)
        {
            Vector3 horizontalLineStart = new Vector3();
            horizontalLineStart.x = bottomLeftCorner.x;
            horizontalLineStart.y = bottomLeftCorner.y + (verticalCell * i);
            horizontalLineStart.z = bottomLeftCorner.z;
            NewLine(horizontalLineStart, new Vector3(topRightCorner.x, horizontalLineStart.y, topRightCorner.z));
        }
    }

    void NewLine(Vector3 start, Vector3 end, string name = "Line")
    {
        GameObject obj = new GameObject(name, typeof(LineRenderer));
        obj.transform.SetParent(transform);

        LineRenderer line = obj.GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPositions(new Vector3[] { start, end });
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.material = lineMaterial;
    }
}
