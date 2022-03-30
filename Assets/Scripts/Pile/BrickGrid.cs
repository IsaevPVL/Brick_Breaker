using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickGrid : MonoBehaviour
{
    int width;
    int height;

    public float cellSize;
    public int arrayWidth;
    public int arrayHeight;
    int[,] gridArray;
    public float horizontalSize;
    public float verticalSize;

    Vector3 origin;

    public GameObject brick;
    public GameObject grid;

    private void Awake() {
        origin = transform.position;
        CreateGrid(arrayWidth, arrayHeight, 1f);

        ListGrid();
    }

    void CreateGrid(int width, int height, float cellSize){
        gridArray = new int[width, height];

    }

    void ListGrid(){
        for(int w = 0; w < gridArray.GetLength(0); w++){
            for(int h = 0; h < gridArray.GetLength(1); h++){
                //Debug.Log(i + " " + j);

                GameObject currentBrick = Instantiate(brick, GetCellPosition(w, h), Quaternion.identity, grid.transform);
                //currentBrick.GetComponent<Brick>().health = Random.Range(1, 4);
            }
        }
    }

    Vector3 GetCellPosition(int width, int height){
        Vector3 cellPosition = new Vector3();
        cellPosition.x = origin.x + width * horizontalSize;
        cellPosition.y = origin.y + height * verticalSize;
        cellPosition.z = origin.z;
        return cellPosition;
    }
}
