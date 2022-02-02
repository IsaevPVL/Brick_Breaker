using UnityEngine;

public class Dragging : MonoBehaviour
{
    #region Variables
    [SerializeField, Range(0, 1)] float timeScaleMultiplyer = 0.2f;
    public Ball ball;

    float distanceZ;
    bool isDragging = false;
    bool isLaunched = false;
    bool isPulling = false;
    Vector3 paddleOffset;
    Vector3 moveTo;
    Vector3 pullTo;
    Transform toDrag;
    Vector3 paddleDefaultPosition;
    Vector3 paddlePositionAtTouch;
    Rigidbody rb;
    Camera cam;

    float width;
    float widthMin;
    float widthMax;

    float fixedDeltaTime;
    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        this.fixedDeltaTime = Time.fixedDeltaTime;

    }

    void Start()
    {
        width = 1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f);
        widthMin = -width / 2f + .7f;
        widthMax = width / 2f - .7f;

        paddleDefaultPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
    }

    void Update()
    {
        if (Input.touchCount == 0)
        {
            isDragging = false;
        }
        else
        {
            Touch touch = Input.touches[0];
            Vector3 touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = cam.ScreenPointToRay(touchPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Paddle"))
                    {
                        toDrag = hit.transform;
                        paddlePositionAtTouch = hit.transform.position;
                        distanceZ = hit.transform.position.z - cam.transform.position.z;
                        
                        Vector3 paddleContactLocation = new Vector3(touchPosition.x, touchPosition.y, distanceZ);
                        paddleContactLocation = cam.ScreenToWorldPoint(paddleContactLocation);
                        //paddleOffset = toDrag.position - paddleContactLocation;
                        paddleOffset = paddlePositionAtTouch - paddleContactLocation;
                        isDragging = true;
                    }
                }
            }

            if (isDragging && touch.phase == TouchPhase.Moved)
            {

                if (Mathf.Abs(touch.deltaPosition.y) > 50f || isPulling)
                {
                    isPulling = true;
                }
                else
                {
                    isPulling = false;
                }

                if (isPulling && !isLaunched)
                {

                    pullTo = new Vector3(toDrag.position.x, touchPosition.y, distanceZ);
                    pullTo = cam.ScreenToWorldPoint(pullTo) + new Vector3(0, paddleOffset.y, 0);
                    pullTo = new Vector3(toDrag.position.x, Mathf.Clamp(pullTo.y, paddleDefaultPosition.y - 0.5f, paddleDefaultPosition.y), pullTo.z);

                    rb.MovePosition(pullTo);
                }

                if (!isPulling)
                {
                    moveTo = new Vector3(touchPosition.x, paddlePositionAtTouch.y, distanceZ);
                    moveTo = cam.ScreenToWorldPoint(moveTo) + new Vector3(paddleOffset.x, 0, 0);
                    moveTo = new Vector3(Mathf.Clamp(moveTo.x, widthMin, widthMax), paddleDefaultPosition.y, moveTo.z);

                    rb.MovePosition(moveTo);
                    //rb.position = moveTo;
                    
                }
            }

            if (isDragging && (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended))
            {
                if (isPulling)
                {
                    rb.MovePosition(new Vector3(rb.position.x, paddleDefaultPosition.y, paddleDefaultPosition.z));
                    if (!isLaunched)
                    {
                        ball.transform.SetParent(null);
                        ball.rb.isKinematic = false;
                        ball.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        //ball.rb.velocity = new Vector3(0, 1 * Mathf.Abs(ball.velocity), 0);
                        //ball.currentDirection = Vector3.up;
                        ball.rb.AddForce(Vector3.up * ball.speed, ForceMode.VelocityChange);
                        //rb.AddForce(new Vector3(0, 1 * Mathf.Abs(ball.velocity), 0), ForceMode.VelocityChange);
                        isLaunched = true;
                    }
                    isPulling = false;
                }
                isDragging = false;
            }
        }

        if (isDragging)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        else if(!isDragging)
        {
            Time.timeScale = timeScaleMultiplyer;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }
}
