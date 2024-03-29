using System.Collections;
using UnityEngine;
using System;

public class TouchableObject : MonoBehaviour
{
    //public TouchManager touchManager;
    [Range(0, 1), SerializeField] float tapWindow = 0.1f;
    public bool canBeDoubleTapped = false;

    public bool isTouched { get; private set; }

    public Vector3 objectDefaultPosition { get; private set; }
    public Vector3 objectPositionAtTouch { get; private set; }
    public Vector3 objectTouchOffset { get; private set; }

    public Vector3 touchPosition { get; private set; }
    int numberOfTaps;
    float tapDuration;
    public GameObject objectTouchEndedOver; //{ get; private set; }
    protected bool isHeld { get; private set; } = false;
    protected bool isTapped { get; private set; } = false;

    public static event Action<string> ObjectWasTapped;
    public static event Action<string> ObjectWasDoubleTapped;


    public virtual void OnEnable()
    {
        TouchManager.ObjectWasTouched += ObjectWasTouched;
        TouchManager.TouchEnded += TouchEnded;

        objectDefaultPosition = transform.position;
    }

    public virtual void OnDisable()
    {
        TouchManager.ObjectWasTouched -= ObjectWasTouched;
        TouchManager.TouchEnded -= TouchEnded;
    }

    void LateUpdate()
    {
        if (isTouched)
        {
            touchPosition = TouchManager.active.currentTouchPosition;

            tapDuration += Time.deltaTime;
            if (!isHeld && tapDuration > 0.2f)
            {
                //Debug.Log(this.name + " is held");
                StopCoroutine(TapsCountdown());
                numberOfTaps = 0;
                isHeld = true;
            }
        }
    }

    void ObjectWasTouched(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            //Debug.Log(this.name);

            objectPositionAtTouch = TouchManager.active.objectPositionAtTouch;
            objectTouchOffset = TouchManager.active.objectTouchOffset;
            touchPosition = TouchManager.active.currentTouchPosition;
            isTouched = true;

            if (!canBeDoubleTapped)
            {
                return;
            }
            numberOfTaps++;
            if (numberOfTaps == 2)
            {
                StopCoroutine(TapsCountdown());
                ObjectWasDoubleTapped?.Invoke(this.name);
                //Debug.Log(this.name + " is doubletapped");
                numberOfTaps = 0;
            }
        }
    }

    void TouchEnded(GameObject objectTouched, GameObject objectTouchEndedOver)
    {
        if (objectTouched == this.gameObject)
        {
            tapDuration = 0;
            isHeld = false;
            isTouched = false;

            this.objectTouchEndedOver = objectTouchEndedOver;

            if (canBeDoubleTapped)
            {
                StartCoroutine(TapsCountdown());
            }
            else
            {
                Tapped();
                //isTapped();
            }

            OnTouchEnd();
        }
    }

    public virtual void OnTouchEnd(){

    }

    void Tapped()
    {
        ObjectWasTapped?.Invoke(this.name);
        //Debug.Log(this.name + " is tapped");
        numberOfTaps = 0;
        isTapped = true;
    }
    public void ResetTap(){
        isTapped = false;
    }

    // public bool isTapped(){
    //     return true;
    // }

    IEnumerator TapsCountdown()
    {
        yield return new WaitForSecondsRealtime(tapWindow);

        // if (numberOfTaps == 1)
        // {
            Tapped();
        // }
        // // else if (numberOfTaps == 2)
        // // {
        // //     StopCoroutine(TapsCountdown());
        // //     ObjectWasDoubleTapped?.Invoke(this.name);
        // //     Debug.Log(this.name + " is doubletapped");
        // // }
    }
}
