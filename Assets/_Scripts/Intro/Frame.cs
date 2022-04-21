
using UnityEngine;

public class Frame : MonoBehaviour
{   
    public Texture2D frame;

    private void OnEnable() {
        DontDestroyOnLoad(this.gameObject);
        TerminalOutputV2.FrameCaptured += SaveFrame;
    }

    private void OnDestroy() {
        TerminalOutputV2.FrameCaptured -= SaveFrame;
    }

    void SaveFrame(Texture2D recievedFrame){
        frame = recievedFrame;
    }
}
