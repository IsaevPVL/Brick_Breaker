using UnityEngine;
using System;

public class PaddleControl : TouchableObject
{
    [SerializeField, Range(0, 1)] float timeScaleMultiplyer = 0.2f;
    [SerializeField] GameObject ball;
    Transform ballDefaultPosition;
    Rigidbody rb;
    bool gotBall;
    Vector3 defaultPosition;
    float fixedDeltaTime;

    public static event Action PaddleDoubleTapped;


    //TEMPORARY
    public float maxAllowedWidth;
    Boundaries boundaries;

    public override void OnEnable()
    {
        base.OnEnable();
        TouchableObject.ObjectWasDoubleTapped += GotTwoTaps;
        HealthManager.BallLoaded += LoadBall;

        ballDefaultPosition = GameObject.Find("Ball Position").GetComponent<Transform>();
        defaultPosition = transform.position;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        TouchableObject.ObjectWasDoubleTapped -= GotTwoTaps;
        HealthManager.BallLoaded -= LoadBall;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fixedDeltaTime = Time.fixedDeltaTime;
        //TouchableObject.ObjectWasDoubleTapped += GotTwoTaps;

        float paddleWidth = GameObject.FindGameObjectWithTag("Hitter").GetComponent<MeshCollider>().bounds.extents.x;
        boundaries = GameObject.FindObjectOfType<Boundaries>();
        maxAllowedWidth = boundaries.corners[1].x - paddleWidth;

    }

    private void OnDestroy()
    {
        //TouchableObject.ObjectWasDoubleTapped -= GotTwoTaps;
        SetTimeScale(1);
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

        float boundX = Mathf.Clamp(touchPosition.x + objectTouchOffset.x, -maxAllowedWidth, maxAllowedWidth);

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

    void LoadBall()
    {
        //Debug.Log("Ball loaded");
        Instantiate(ball, ballDefaultPosition.position, Quaternion.identity).GetComponent<Transform>().SetParent(transform);
        gotBall = true;
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