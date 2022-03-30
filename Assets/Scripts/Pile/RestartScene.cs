using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public void RstartScene(){
        SceneManager.LoadSceneAsync(0);
    }
}