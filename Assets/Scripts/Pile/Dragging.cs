using UnityEngine;
using System.Collections;

public class Dragging : MonoBehaviour
{
    #region Variables
    [SerializeField, Range(0, 1)] float timeScaleMultiplyer = 0.2f;
    public Ball ball;
    public float afterLaunchLerp = 1f;

    float distanceZ;
    bool isDragging = false;
    bool isLaunched = false;
    Vector3 paddleOffset;
    Vector3 moveTo;
    Vector3 pullTo;
    Transform toDrag;
    Vector3 paddleDefaultPosition;
    Quaternion paddleDefaultRotation;
    Vector3 paddlePositionAtTouch;
    Rigidbody rb;
    Camera cam;
    //int tapCount;
    bool tappedOnce;

    float width;
    float widthMin;
    float widthMax;

    float fixedDeltaTime;

    //Direction Change
    bool isChangingDirection;
    Vector3 directionCentre;
    Vector3 desiredDirection;

    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        this.fixedDeltaTime = Time.fixedDeltaTime;

        width = 1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f);
        widthMin = -width / 2f + .7f;
        widthMax = width / 2f - .7f;

        paddleDefaultPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        paddleDefaultRotation = transform.rotation;
        ResetPaddle();
    }

    void Start()
    {

    }

    void Update()
    {
        int PostProsessinglayerMask = 3 << 8;
        PostProsessinglayerMask = ~PostProsessinglayerMask;

        if (Input.touchCount == 0)
        {
            isDragging = false;
            isChangingDirection = false;
        }
        else
        {
            Touch touch = Input.touches[0];
            Vector3 touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = cam.ScreenPointToRay(touchPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, PostProsessinglayerMask))
                {
                    distanceZ = hit.transform.position.z - cam.transform.position.z;

                    if (hit.collider.CompareTag("Paddle"))
                    {
                        toDrag = hit.transform;
                        paddlePositionAtTouch = hit.transform.position;
                        //distanceZ = hit.transform.position.z - cam.transform.position.z;

                        Vector3 paddleContactLocation = new Vector3(touchPosition.x, touchPosition.y, distanceZ);
                        paddleContactLocation = cam.ScreenToWorldPoint(paddleContactLocation);
                        paddleOffset = paddlePositionAtTouch - paddleContactLocation;
                        isDragging = true;

                        if (!isLaunched)
                        {
                            if (tappedOnce)
                            {
                                LaunchBall();
                                tappedOnce = false;
                                StopCoroutine(DoubleTapCountdown());
                            }
                            tappedOnce = true;
                            StartCoroutine(DoubleTapCountdown());
                        }
                    }
                    else if (hit.collider.CompareTag("Direction Change"))
                    {
                        isChangingDirection = true;
                        directionCentre = hit.collider.transform.position;
                    }
                }
            }

            if (isDragging && touch.phase == TouchPhase.Moved)
            {
                moveTo = new Vector3(touchPosition.x, paddlePositionAtTouch.y, distanceZ);
                moveTo = cam.ScreenToWorldPoint(moveTo) + new Vector3(paddleOffset.x, 0, 0);
                moveTo = new Vector3(Mathf.Clamp(moveTo.x, widthMin, widthMax), paddleDefaultPosition.y, moveTo.z);

                rb.MovePosition(moveTo);
            }

            if (isChangingDirection && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
            {
                desiredDirection = cam.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, distanceZ));

                Vector3 ballOffset = ball.transform.position - directionCentre;

                desiredDirection = (desiredDirection + ballOffset) - ball.transform.position;

                Debug.Log(desiredDirection.magnitude);

            }

            if (isChangingDirection && touch.phase == TouchPhase.Ended)
            {
                ball.GetComponent<Rigidbody>().velocity = desiredDirection.normalized * ball.speed;
            }
        }

        //Slow-motion
        if (isDragging || !ball.isCollidedAfterLaunch)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        else
        {
            Time.timeScale = timeScaleMultiplyer;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }

    void ResetPaddle()
    {
        transform.position = paddleDefaultPosition;
        transform.rotation = paddleDefaultRotation;
        isLaunched = false;
    }

    void LaunchBall()
    {
        ball.transform.SetParent(null);
        ball.rb.isKinematic = false;
        ball.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        //ball.rb.velocity = new Vector3(0, 1 * Mathf.Abs(ball.velocity), 0);
        ball.rb.AddForce(Vector3.up * ball.speed, ForceMode.VelocityChange);
        isLaunched = true;
    }

    private IEnumerator DoubleTapCountdown()
    {
        yield return new WaitForSeconds(0.3f);
        tappedOnce = false;
    }
}