using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<Vector3Int, string> inventory; 

    public GameData()
    {
        inventory = new SerializableDictionary<Vector3Int, string>();
    }
}
