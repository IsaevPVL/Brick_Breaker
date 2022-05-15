using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileElement : MonoBehaviour, IDataPersistence
{
    public string id;
    [Space]
    public GameObject file;

    [Space, Header("Visual Elements")]
    [SerializeField] TextMeshProUGUI fileName;
    [SerializeField] TextMeshProUGUI fileAmount;
    [SerializeField] TextMeshProUGUI fileXSize;
    [SerializeField] TextMeshProUGUI fileYSize;

    [Space]
    public int maxAmount;
    public int availableAmount;

    //Dictionary<Vector3Int, string> inventory = new Dictionary<Vector3Int, string>(InventoryGrid.active.inventoryDimensions.x * InventoryGrid.active.inventoryDimensions.y);
    public List<Vector3Int> children = new List<Vector3Int>(10);


    void Awake()
    {
        availableAmount = maxAmount;

        fileName.text = file.name;
        PlaceableObject thisObject = file.GetComponent<PlaceableObject>();
        fileXSize.text = thisObject.dimensions.x.ToString();
        fileYSize.text = thisObject.dimensions.y.ToString();
        SetAmount();

    }

    private void Start()
    {
        foreach (Vector3Int child in children)
        {
            PutOnGrid(child);
        }
    }

    public void TryFindPlaceOnGrid()
    {
        if (availableAmount == 0)
        {
            print("No more available!");
            return;
        }

        GameObject fileToPlace = Instantiate(file, Vector2.one * -100f, Quaternion.identity);
        PlaceableObject createdObject = fileToPlace.GetComponent<PlaceableObject>();
        createdObject.myCurator = this;

        if (createdObject.FindPlaceOnGrid())
        {
            //children.Add(createdObject.defaultCell);
            //SetAmount();
            return;
        }

        print("No place!");
        Destroy(fileToPlace);
    }

    public void ReturnTo(Vector3Int child)
    {
        children.Remove(child);
        SetAmount();
    }
    public void Move(Vector3Int oldCell, Vector3Int newCell){
        children.Remove(oldCell);
        children.Add(newCell);
        SetAmount();
    }

    void SetAmount()
    {
        availableAmount = maxAmount - children.Count;
        fileAmount.text = $"({availableAmount})";
    }

    void PutOnGrid(Vector3Int cell)
    {
        GameObject fileToPlace = Instantiate(file, Vector2.one * -100f, Quaternion.identity);
        PlaceableObject createdObject = fileToPlace.GetComponent<PlaceableObject>();
        createdObject.myCurator = this;
        createdObject.defaultCell = cell;
        InventoryGrid.active.PlaceObject(createdObject, true);
    }

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<Vector3Int, string> kv in data.inventory)
        {
            if (kv.Value == id)
            {
                children.Add(kv.Key);
            }
        }

        // foreach (Vector3Int child in children)
        // {
        //     PutOnGrid(child);
        // }

        // if (data.inventory.TryGetValue(id, out Vector3Int[] arr))
        // {
        //     children = new List<Vector3Int>(arr);

        //     // foreach (Vector3Int child in children)
        //     // {
        //     //     PutOnGrid(child);
        //     // }
        // }
        // else
        // {
        //     children = new List<Vector3Int>(10);
        // }

        SetAmount();
    }

    public void SaveData(GameData data)
    {

        foreach (Vector3Int child in children)
        {
            //if(!data.inventory.ContainsKey(child)){
                data.inventory.Add(child, id);
            //}
        }

        // if (data.inventory.ContainsKey(id))
        // {
        //     data.inventory[id] = children.ToArray();
        // }
        // else
        // {
        //     data.inventory.Add(id, children.ToArray());
        // }
    }
}