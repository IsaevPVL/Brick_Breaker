using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : TouchableObject
{
    private void Update()
    {
        if (!isTouched)
        {
            return;
        }

        if(GridSystem.active.WithinGridBoundaries(touchPosition + objectTouchOffset)){
            transform.position = GridSystem.active.SnapCordinateToGrid();
        }
        //transform.position = GridSystem.current.SnapCordinateToGrid(touchPosition);
    }
}
