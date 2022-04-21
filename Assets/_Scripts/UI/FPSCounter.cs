using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text fpsText;

    float fps;

    private void Awake() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        float fps = 1 / Time.unscaledDeltaTime;
        fpsText.text = "" + fps;
    }
}
