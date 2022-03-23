using UnityEngine;
using System;

public class TouchManager : MonoBehaviour
{
    public static TouchManager active { get; private set; }

    public static event Action<GameObject> ObjectWasTouched;
    public static event Action TouchEnded;

    Camera cam;
    Touch touch;
    RaycastHit objectTouched;

    float positionZ;
    public Vector3 objectPositionAtTouch;
    Vector3 objectTouchLocation;

    public Vector3 objectTouchOffset;
    public Vector3 currentTouchPosition;
    public bool touchMoved;

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
        //positionZ = GameObject.Find("Paddle").transform.position.z - cam.transform.position.z;
    }

    // private void Start()
    // {
    // }

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
            TouchEnded?.Invoke();
        }
    }

    void GetTouchPositionInWorldSpace()
    {
        currentTouchPosition = new Vector3(touch.position.x, touch.position.y, positionZ);
        currentTouchPosition = cam.ScreenToWorldPoint(currentTouchPosition);
    }

    // public bool Touched()
    // {
    //     return (Input.touchCount == 0) ? false : true;
    // }
}