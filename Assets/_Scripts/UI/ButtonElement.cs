using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonElement : TouchableObject
{
    // void Start()
    // {
    //     TouchableObject.ObjectWasTapped += RestartScene;
    // }
    // // private void OnDisable()
    // // {
    // //     TouchableObject.ObjectWasTapped -= RestartScene;
    // // }
    // private void OnDestroy()
    // {
    //     TouchableObject.ObjectWasTapped -= RestartScene;
    // }
    private void Update() {
        if(isTouched){
            RestartScene("Restart");
        }
    }

    void RestartScene(string name)
    {
        if (name == "Restart")
        {
            //SceneManager.MergeScenes(SceneManager.GetActiveScene(), SceneManager.CreateScene(Time.time.ToString()));
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
    }
}