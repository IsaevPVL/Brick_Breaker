using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class PlaceableObject : TouchableObject
{
    public Vector3Int defaultCell;
    [SerializeField] float zThickness = 0.2f;
    [SerializeField] protected bool isUnlocked = true;
    public Transform visual;
    [SerializeField] GameObject text;
    [SerializeField] GameObject deleteIcon = null;
    public Vector2Int dimensions;

    public Stack<Vector3Int> occupiedCells;
    public List<PlaceableObject> objectsConnectedTo;

#pragma warning disable
    public virtual event Action ProgramTriggered;
#pragma warning restore


    public override void OnEnable()
    {
        base.OnEnable();
        InventoryGrid.NewScale += NewScale;
        //InventoryGrid.ObjectPlaced += GetConnections;
        UIManager.StateChanged += ApplyCurrentState;

        occupiedCells = new Stack<Vector3Int>(dimensions.x * dimensions.y);
        objectsConnectedTo = new List<PlaceableObject>();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        InventoryGrid.NewScale -= NewScale;
        //InventoryGrid.ObjectPlaced -= GetConnections;
        UIManager.StateChanged -= ApplyCurrentState;
    }

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.1f); //BAD!!!
        //NewScale(new Vector2(InventoryGrid.active.horizontalSector * dimensions.x, InventoryGrid.active.verticalSector * dimensions.y));

        InventoryGrid.active.PlaceObject(this, true);
        ApplyCurrentState(GameObject.FindObjectOfType<UIManager>().currentState);
    }

    public virtual void Update()
    {
        if (!isTouched)
        {
            return;
        }

        if (isUnlocked)
        {
            if (InventoryGrid.active.PlaceObject(this))
            {
                GetConnections();

                // if (objectsConnectedTo.Count > 0)
                // {
                //     SubscribeToConnections();
                // }
            }

        }
    }

    //Iportant, don't delete!
    protected virtual void SubscribeToConnection(PlaceableObject obj)
    {

    }

    protected virtual void UnsubscribeFromConnection(PlaceableObject obj)
    {

    }

    protected virtual void SubscribeToConnections()
    {

    }
    //-----------------------
    public bool FindPlaceOnGrid()
    {
        for (int y = 2; y <= InventoryGrid.active.inventoryDimensions.y - dimensions.y; y++)
        {
            defaultCell.y = y;

            for (int x = 0; x <= InventoryGrid.active.inventoryDimensions.x - dimensions.x; x++)
            {
                defaultCell.x = x;

                if (InventoryGrid.active.PlaceObject(this, true))
                {   
                    GetConnections();
                    return true;
                }
            }
        }
        return false;
    }

    void GetConnections()
    {
        foreach (PlaceableObject neighbour in objectsConnectedTo)
        {
            UnsubscribeFromConnection(neighbour);
            neighbour.objectsConnectedTo.Remove(this);
        }
        objectsConnectedTo.Clear();
        PlaceableObject obj;

        foreach (Vector3Int thisCell in occupiedCells)
        {
            if (InventoryGrid.active.cellFree.TryGetValue(new Vector3Int(thisCell.x - 1, thisCell.y, thisCell.z), out obj))
            {
                ConnectObjects();
            }
            if (InventoryGrid.active.cellFree.TryGetValue(new Vector3Int(thisCell.x + 1, thisCell.y, thisCell.z), out obj))
            {
                ConnectObjects();
            }
            if (InventoryGrid.active.cellFree.TryGetValue(new Vector3Int(thisCell.x, thisCell.y - 1, thisCell.z), out obj))
            {
                ConnectObjects();
            }
            if (InventoryGrid.active.cellFree.TryGetValue(new Vector3Int(thisCell.x, thisCell.y + 1, thisCell.z), out obj))
            {
                ConnectObjects();
            }
        }

        void ConnectObjects()
        {
            if (obj != null && obj != this && !objectsConnectedTo.Contains(obj))
            {
                SubscribeToConnection(obj);

                objectsConnectedTo.Add(obj);
                obj.objectsConnectedTo.Add(this);
            }
        }
    }

    // void GetConnections(PlaceableObject obj)
    // {
    //     // if(obj == this){
    //     //     return;
    //     // }
    //     // print(this.name + " " + occupiedCells.Count);

    //     // foreach (Vector3Int otherCell in obj.occupiedCells)
    //     // {
    //     //     foreach (Vector3Int thisCell in occupiedCells)
    //     //     {
    //     //         if (thisCell.y == otherCell.y)
    //     //         {
    //     //             //print(this.name + " same y");
    //     //             if(thisCell.x == otherCell.x + 1 || thisCell.x == otherCell.x - 1){
    //     //                 objectsConnectedTo.Add(obj);
    //     //                 print(objectsConnectedTo.Count);
    //     //             }

    //     //         }
    //     //         else if (thisCell.y == otherCell.y + 1 || thisCell.y == otherCell.y - 1)
    //     //         {
    //     //             //print(this.name + " neighbouring y");
    //     //             if(thisCell.x == otherCell.x){
    //     //                 objectsConnectedTo.Add(obj);
    //     //             }
    //     //         }else if(objectsConnectedTo.Contains(obj)){
    //     //             objectsConnectedTo.Remove(obj);
    //     //         }
    //     //     }
    //     // }
    // }

    void NewScale(Vector2 scale)
    {
        scale *= dimensions;
        BoxCollider box = this.gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(scale.x, scale.y, zThickness);
        box.center = new Vector3(scale.x / 2, scale.y / 2, -zThickness / 2);

        if (visual != null)
        {
            visual.localScale = new Vector3(scale.x, scale.y, zThickness);
        }

        if (text != null)
        {
            text.transform.localPosition = new Vector3(0, 0, -zThickness - 0.01f);
            text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scale.x);
            text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scale.y);
        }

        if(deleteIcon != null){
            deleteIcon.transform.localPosition = new Vector3(scale.x, scale.y, -zThickness - 0.01f);
        }
    }

    void ApplyCurrentState(UIState currentState)
    {
        // isUnlocked = (currentState == UIState.Files) ? true : false;

        if (currentState == UIState.Files)
        {
            isUnlocked = true;
            //deleteIcon?.SetActive(true);
        }
        else
        {
            isUnlocked = false;
            //deleteIcon?.SetActive(false);
        }
    }
}