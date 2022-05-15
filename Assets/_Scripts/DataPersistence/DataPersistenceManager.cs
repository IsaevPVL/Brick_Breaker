using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{   
    [Space, SerializeField] string fileName;

    public static DataPersistenceManager active { get; private set; }

    GameData gameData;
    List<IDataPersistence> dataPersistenceObjects;
    FileDataHandler dataHandler;

    private void Awake()
    {
        #region Singleton stuff
        if (active != null && active != this)
        {
            Destroy(this);
        }
        else
        {
            active = this;
        }
        #endregion

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("No previous data found, creating new");
            NewGame();
        }

        foreach (IDataPersistence obj in dataPersistenceObjects)
        {
            obj.LoadData(gameData);
        }

        gameData.inventory.Clear();
    }

    public void SaveGame()
    {
        foreach (IDataPersistence obj in dataPersistenceObjects)
        {
            obj.SaveData(gameData);
        }

        dataHandler.Save(gameData);
    }

    List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}