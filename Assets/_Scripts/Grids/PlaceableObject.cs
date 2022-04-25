using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class PlaceableObject : TouchableObject
{
    public Vector3Int defaultCell;
    [SerializeField] bool isUnlocked = true;
    public Transform visual;
    [SerializeField] GameObject text;
    public Vector2Int dimensions;

    public Stack<Vector3Int> occupiedCells = new Stack<Vector3Int>();

    public override void OnEnable()
    {
        base.OnEnable();
        InventoryGrid.NewScale += NewScale;
    }
    public override void OnDisable()
    {   
        base.OnDisable();
        InventoryGrid.NewScale -= NewScale;
    }

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        //NewScale(new Vector2(InventoryGrid.active.horizontalSector * dimensions.x, InventoryGrid.active.verticalSector * dimensions.y));

        InventoryGrid.active.PlaceObject(this, true);
    }

    public virtual void Update()
    {
        if (!isTouched)
        {
            return;
        }
        if(isUnlocked){
            InventoryGrid.active.PlaceObject(this);
        }
    }

    void NewScale(Vector2 scale)
    {
        scale *= dimensions;
        BoxCollider box = this.gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(scale.x, scale.y, 0.2f);
        box.center = new Vector3(scale.x / 2, scale.y / 2, -0.1f);

        if (visual != null)
        {
            visual.localScale = new Vector3(scale.x, scale.y, 0.2f);
        }

        if (text != null)
        {
            text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scale.x);
            text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scale.y);
        }
    }
}