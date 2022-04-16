using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FastForward : MonoBehaviour
{
    //[SerializeField, Range(1, 100)] float fastForwardMultiplier = 1.5f;
    [SerializeField, Range(0, 10)] float skipDelay = 2f;
    [SerializeField] GameObject imageFastForward;
    [SerializeField] Image imageProgress;
    [SerializeField] GameObject instructions;
    bool fastForwarding = false;
    Vector3 defaultPosition;

    TerminalOutputV2 output;

    private void Start() {
        output = FindObjectOfType<TerminalOutputV2>();
        imageProgress.fillAmount = 0f;
        imageFastForward.SetActive(false);
        instructions.SetActive(false);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {

            if (fastForwarding)
            {
                imageProgress.fillAmount += 1f / skipDelay * Time.deltaTime;

                if(imageProgress.fillAmount == 1f){
                    output.StartGame();
                }

                return;
            }
            fastForwarding = true;
            //Time.timeScale = fastForwardMultiplier;
            //output.characterDelay = 0;
            //output.fastForwardMultiplier = 0;
            //imageFastForward.SetActive(true);
            //imageFastForward.GetComponent<RectTransform>().DOPivotX(1.2f, 0.2f).SetLoops(-1, LoopType.Yoyo);
            instructions.SetActive(true);
            instructions.GetComponent<RectTransform>().DOPivotX(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            if (!fastForwarding)
            {
                return;
            }
            fastForwarding = false;
            //Time.timeScale = 1;
            //output.characterDelay = 0.02f;
            //output.fastForwardMultiplier = 1;
            //imageFastForward.transform.DORewind(false);
            //imageFastForward.SetActive(false);
            instructions.transform.DORewind(false);
            instructions.SetActive(false);
            imageProgress.fillAmount = 0f;
        }
    }

    // private void OnDisable()
    // {
    //     Time.timeScale = 1;
    //     imageFastForward.transform.DOKill(false);
    // }

    private void OnDestroy()
    {
        Time.timeScale = 1;
        //imageFastForward.transform.DOKill(false);
    }
}
