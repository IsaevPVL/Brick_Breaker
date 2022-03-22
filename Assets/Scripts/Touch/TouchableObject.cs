using System.Collections;
using UnityEngine;

public class TouchableObject : MonoBehaviour
{
    //public TouchManager touchManager;
    public bool isTouched{ get; private set; }

    public Vector3 objectDefaultPosition{ get; private set; }
    public Vector3 objectPositionAtTouch{ get; private set; }

    public Vector3 objectTouchOffset{ get; private set; }

    public Vector3 touchPosition; //{ get; private set; }

    bool objectTappedOnce;
    public bool objectDoubleTapped{ get; set; }


    private void OnEnable()
    {
        TouchManager.ObjectWasTouched += ObjectWasTouched;
        TouchManager.TouchEnded += TouchEnded;

        objectDefaultPosition = transform.position;
    }

    private void OnDisable()
    {
        TouchManager.ObjectWasTouched -= ObjectWasTouched;
        TouchManager.TouchEnded -= TouchEnded;
    }

    private void LateUpdate()
    {
        if (isTouched)
        {
            touchPosition = TouchManager.active.currentTouchPosition;
        }
    }

    void ObjectWasTouched(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            Debug.Log(this.name);

            objectPositionAtTouch = TouchManager.active.objectPositionAtTouch;
            objectTouchOffset = TouchManager.active.objectTouchOffset;
            touchPosition = TouchManager.active.currentTouchPosition;

            if (objectTappedOnce)
            {
                objectDoubleTapped = true;
                StopCoroutine(DoubleTapCountdown());
            }
            objectTappedOnce = true;
            StartCoroutine(DoubleTapCountdown());

            isTouched = true;
        }
    }

    void TouchEnded()
    {
        isTouched = false;
    }

    private IEnumerator DoubleTapCountdown()
    {
        yield return new WaitForSeconds(0.3f);
        objectTappedOnce = false;
    }
}
