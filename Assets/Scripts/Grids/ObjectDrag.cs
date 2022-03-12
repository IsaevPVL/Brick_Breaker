using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    Vector3 offset;

    private void OnMouseDown() {
        offset = transform.position - GridSystem.GetTouchWorldPosition();
    }

    private void OnMouseDrag() {
        Vector3 pos = GridSystem.GetTouchWorldPosition() + offset;
        transform.position = GridSystem.current.SnapCordinateToGrid(pos);
    }
}
