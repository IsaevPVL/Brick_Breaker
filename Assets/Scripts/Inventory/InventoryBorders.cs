using UnityEngine;
using System.Collections;

public class InventoryBorders : MonoBehaviour
{
    public float lineWidth = 0.05f;

    public GameObject top;
    public GameObject right;
    public GameObject bottom;
    public GameObject left;

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.1f); //CHANGE!!!?

        Vector3 bottomLeftCorner = GridSystem.active.inventoryBottomLeftCorner;
        Vector3 topRightCorner = GridSystem.active.inventoryTopRightCorner;
        Vector3 topLeftCorner = new Vector3(bottomLeftCorner.x, topRightCorner.y, 0);
        Vector3 bottomRightCorner = new Vector3(topRightCorner.x, bottomLeftCorner.y, 0);

        SetLine(top, topLeftCorner, topRightCorner);
        SetLine(right, topRightCorner, bottomRightCorner);
        SetLine(bottom, bottomRightCorner, bottomLeftCorner);
        SetLine(left, bottomLeftCorner, topLeftCorner);
    }

    void SetLine(GameObject segment, Vector3 start, Vector3 end)
    {
        Vector3[] endpoints = new Vector3[] { start, end };
        LineRenderer line = segment.GetComponent<LineRenderer>();
        line.positionCount = endpoints.Length;
        line.SetPositions(endpoints);
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
    }
}
