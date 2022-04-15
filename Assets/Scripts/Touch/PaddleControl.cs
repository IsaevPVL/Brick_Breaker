using UnityEngine;
using System;

public class PaddleControl : TouchableObject
{
    Rigidbody rb;
    bool gotBall = true;

    [SerializeField, Range(0, 1)] float timeScaleMultiplyer = 0.2f;
    float fixedDeltaTime;

    public static event Action PaddleDoubleTapped;

    //TEMPORARY
    float widthMin = -5.5f;
    float widthMax = 5.5f;
    Boundaries boundaries;

    //private void OnEnable()
    // {
    //   TouchableObject.ObjectWasDoubleTapped += GotTwoTaps;
    //  }

    // private void OnDisable()
    // {
    //     TouchableObject.ObjectWasDoubleTapped -= GotTwoTaps;
    // }

    private void OnDestroy()
    {
        TouchableObject.ObjectWasDoubleTapped -= GotTwoTaps;
        SetTimeScale(1);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fixedDeltaTime = Time.fixedDeltaTime;

        TouchableObject.ObjectWasDoubleTapped += GotTwoTaps;

        float paddleWidth = GameObject.FindGameObjectWithTag("Hitter").GetComponent<MeshCollider>().bounds.extents.x;
        boundaries = GameObject.FindObjectOfType<Boundaries>();
        widthMin = boundaries.corners[0].x + paddleWidth;
        widthMax = boundaries.corners[1].x - paddleWidth;
    }

    private void Update()
    {
        if (!isTouched)
        {
            if (Time.timeScale != timeScaleMultiplyer && Time.timeScale != 0)
            {
                SetTimeScale(timeScaleMultiplyer);
            }
            return;
        }
        if (Time.timeScale != 1)
        {
            SetTimeScale(1);
        }

        float boundX = Mathf.Clamp(touchPosition.x + objectTouchOffset.x, widthMin, widthMax);

        //Vector3 moveTo = new Vector3(boundX, objectDefaultPosition.y, objectDefaultPosition.z) + new Vector3(objectTouchOffset.x, 0f, 0f);
        Vector3 moveTo = new Vector3(boundX, objectDefaultPosition.y, objectDefaultPosition.z);

        rb.MovePosition(moveTo);

        // if (objectDoubleTapped)
        // {
        //     Debug.Log(name + " double tapped");
        //     PaddleDoubleTapped?.Invoke();
        //     objectDoubleTapped = false;
        // }

    }

    void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
    }

    void GotTwoTaps(string obj)
    {
        if (gotBall && obj == "Paddle")
        {
            PaddleDoubleTapped?.Invoke();
            gotBall = false;
        }
    }
}