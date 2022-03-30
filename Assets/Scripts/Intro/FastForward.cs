using UnityEngine;
using DG.Tweening;

public class FastForward : MonoBehaviour
{
    [SerializeField, Range(1, 100)] float fastForwardMultiplier = 1.5f;
    [SerializeField] GameObject image;
    bool fastForwarding = false;
    Vector3 defaultPosition;

    private void Update()
    {
        if (Input.touchCount > 0)
        {

            if (fastForwarding)
            {
                return;
            }
            fastForwarding = true;
            Time.timeScale = fastForwardMultiplier;
            image.SetActive(true);
            image.GetComponent<RectTransform>().DOPivotX(1.2f, 0.2f).SetLoops(-1, LoopType.Yoyo);
            
        }
        else
        {
            if (!fastForwarding)
            {
                return;
            }
            fastForwarding = false;
            Time.timeScale = 1;
            image.transform.DORewind(false);
            image.SetActive(false);
        }
    }

    // private void OnDisable()
    // {
    //     Time.timeScale = 1;
    //     image.transform.DOKill(false);
    // }

    private void OnDestroy()
    {
        Time.timeScale = 1;
        //image.transform.DOKill(false);
    }
}
