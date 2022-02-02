using UnityEngine;

public class ScreenTest : MonoBehaviour
{
    private void Awake() {
        Camera cam = Camera.main;

        float aspectRatio = (float)Screen.height / Screen.width;
        Debug.Log(aspectRatio);
    }


}
