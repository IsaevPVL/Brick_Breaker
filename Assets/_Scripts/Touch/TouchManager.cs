using UnityEngine;
using System;

public class TouchManager : MonoBehaviour
{
    public static TouchManager active { get; private set; }

    public static event Action<GameObject> ObjectWasTouched;
    public static event Action<GameObject> TouchEnded;

    Camera cam;
    
    Touch touch;
    RaycastHit objectTouched;
    float positionZ;
    public Vector3 objectPositionAtTouch { get; private set; }
    public Vector3 objectTouchOffset { get; private set; }
    public Vector3 currentTouchPosition { get; private set; }
    public bool touchMoved { get; private set; }
    Vector3 objectTouchLocation;

    //Creating instance and ensuring it is the only one
    private void Awake()
    {
        if (active != null && active != this)
        {
            Destroy(this);
        }
        else
        {
            active = this;
        }
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        touch = Input.touches[0];
        GetTouchPositionInWorldSpace();

        if (touch.phase == TouchPhase.Began)
        {

            Ray ray = cam.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out objectTouched))
            {   
                Debug.Log(objectTouched.collider.name + " is touched");
                positionZ = objectTouched.transform.position.z - cam.transform.position.z;

                objectPositionAtTouch = objectTouched.transform.position;
                objectTouchLocation = new Vector3(touch.position.x, touch.position.y, positionZ);
                objectTouchLocation = cam.ScreenToWorldPoint(objectTouchLocation);
                objectTouchOffset = objectPositionAtTouch - objectTouchLocation;

                ObjectWasTouched?.Invoke(objectTouched.transform.gameObject);
            }
        }

        if (touch.phase == TouchPhase.Moved)
        {
            GetTouchPositionInWorldSpace();
            touchMoved = true;
        }

        if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
        {
            touchMoved = false;

            if (objectTouched.transform != null)
            {
                TouchEnded?.Invoke(objectTouched.transform.gameObject);
            }
        }
    }

    void GetTouchPositionInWorldSpace()
    {
        currentTouchPosition = new Vector3(touch.position.x, touch.position.y, positionZ);
        currentTouchPosition = cam.ScreenToWorldPoint(currentTouchPosition);
    }
}