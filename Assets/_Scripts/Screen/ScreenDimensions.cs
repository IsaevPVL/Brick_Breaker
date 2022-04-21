using UnityEngine;

public class ScreenDimensions : MonoBehaviour
{
    public static ScreenDimensions active { get; private set; }

    Camera cam;

    float screenWidth;
    float screenHeight;

    public Vector3 topLeftCorner;
    public Vector3 topRightCorner;
    public Vector3 bottomRightCorner;
    public Vector3 bottomLeftCorner;


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

    private void Start()
    {
        FindBoundaries();
    }

    void FindBoundaries()
    {
        float distanceToCamera = GameObject.Find("Paddle").transform.position.z - cam.transform.position.z;

        topLeftCorner = cam.ViewportToWorldPoint(new Vector3(0, 1, distanceToCamera));
        topRightCorner = cam.ViewportToWorldPoint(new Vector3(1, 1, distanceToCamera));
        bottomRightCorner = cam.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        bottomLeftCorner = cam.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
    }
}
