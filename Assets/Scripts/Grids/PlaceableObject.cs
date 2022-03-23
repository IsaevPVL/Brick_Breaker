using UnityEngine;
using System.Collections;
using System;

public class PlaceableObject : TouchableObject
{
    public Vector3Int defaultCell;
    public bool isUnlocked = true;
    public Transform visual;
    public GameObject text;

    // IEnumerator OnEnable()
    // {
    //     GridSystem.NewScale += NewScale;
    // }
    // private void OnDisable()
    // {
    //     GridSystem.NewScale -= NewScale;
    // }

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        NewScale(new Vector2(GridSystem.active.horizontalSector, GridSystem.active.verticalSector));

        transform.position = GridSystem.active.inventoryGridLayout.CellToWorld(defaultCell);
    }

    private void Update()
    {
        if (!isTouched)
        {
            return;
        }

        if (isUnlocked && GridSystem.active.WithinGridBoundaries(touchPosition + objectTouchOffset))
        {
            transform.position = GridSystem.active.SnapCordinateToGrid();
        }
    }

    void NewScale(Vector2 scale)
    {
        BoxCollider box = this.gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(scale.x, scale.y, 0.2f);
        box.center = new Vector3(scale.x / 2, scale.y / 2, -0.1f);

        if (visual != null)
        {
            visual.localScale = new Vector3(scale.x, scale.y, 0.2f);
        }

        if (text != null)
        {
            //text.GetComponent<RectTransform>().position = transform.position;
            //Debug.Log(text.GetComponent<RectTransform>().rect);
            text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scale.x);
            text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scale.y);
        }
    }
}