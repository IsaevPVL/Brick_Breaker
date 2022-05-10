using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileElement : MonoBehaviour
{
    public GameObject file;
    public int maxAmount;
    int availableAmount;


    void Awake() {
        availableAmount = maxAmount;
    }

    public void TryFindPlaceOnGrid()
    {   
        if(availableAmount == 0){
            print("No more available!");
            return;
        }

        GameObject fileToPlace = Instantiate(file, Vector2.one * -100f, Quaternion.identity);
        PlaceableObject obj = fileToPlace.GetComponent<PlaceableObject>();
        
        if(obj.FindPlaceOnGrid()){
            availableAmount--;
            return;
        }

        print("No place!");
        Destroy(fileToPlace);
    }
} 