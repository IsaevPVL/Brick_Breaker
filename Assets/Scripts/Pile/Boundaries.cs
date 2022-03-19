using UnityEngine;

public class Boundaries : MonoBehaviour
{
    Camera cam;
    public float width;
    public float height;

    public float offset = 0.1f;
    public float lineWidth = 0.05f;
    [Space]

    [Header("Offset in %")]
    [Range(0, 1)] public float topOffset;
    [Range(0, 1)] public float bottomOffset;
    [Range(0, 1)] public float sidesOffset;

    [Space]
    public GameObject top;
    public GameObject right;
    public GameObject bottom;
    public GameObject left;

    public Vector3 topLeftCorner;
    public Vector3 topRightCorner;
    public Vector3 bottomRightCorner;
    public Vector3 bottomLeftCorner;

    public Vector3[] corners = new Vector3[4];

    private void Awake()
    {
        cam = Camera.main;
        //FindBoundaries();
        FindScreenBoundaries();
        corners = GetPlayBoundaries();

        SetLine(top, new Mesh(), corners[0], corners[1]);
        top.transform.position = new Vector3(top.transform.position.x, height, height);

        SetLine(right, new Mesh(), corners[1], corners[2]);
        right.transform.position = new Vector3(-width, right.transform.position.y, -width);

        LineRenderer bottomLine = SetLine(bottom, new Mesh(), corners[2], corners[3]);
        bottom.transform.position = new Vector3(bottom.transform.position.x, -height, height);
        // + 10f temporary value to offset from the bottom
        //bottomLine.startColor = Color.red;
        //bottomLine.endColor = Color.red;

        SetLine(left, new Mesh(), corners[3], corners[0]);
        left.transform.position = new Vector3(width, left.transform.position.y, -width);

    }

    void FindScreenBoundaries()
    {
        float distanceToCamera = GameObject.FindGameObjectWithTag("Paddle").transform.position.z - cam.transform.position.z;

        topLeftCorner = cam.ViewportToWorldPoint(new Vector3(0, 1, distanceToCamera));
        topRightCorner = cam.ViewportToWorldPoint(new Vector3(1, 1, distanceToCamera));
        bottomRightCorner = cam.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        bottomLeftCorner = cam.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
    }

    Vector3[] GetPlayBoundaries(){
        Vector3[] playBoundaries = new Vector3[4];

        float screenHeight = topLeftCorner.y - bottomLeftCorner.y;
        float screenWidth = topLeftCorner.x - topRightCorner.x;
        Debug.Log(screenWidth);

        float topValue = screenHeight * topOffset;
        float bottomValue = screenHeight * bottomOffset;
        float sidesValue = screenWidth * sidesOffset;

        playBoundaries[0] = new Vector3(topLeftCorner.x - sidesValue, topLeftCorner.y - topValue, topLeftCorner.z);
        playBoundaries[1] = new Vector3(topRightCorner.x + sidesValue, topRightCorner.y - topValue, topRightCorner.z);
        playBoundaries[2] = new Vector3(bottomRightCorner.x + sidesValue, bottomRightCorner.y + bottomValue, bottomRightCorner.z);
        playBoundaries[3] = new Vector3(bottomLeftCorner.x - sidesValue, bottomLeftCorner.y + bottomValue, bottomLeftCorner.z);

        height = (playBoundaries[0].y - playBoundaries[3].y) / 2;
        width = (playBoundaries[0].x - playBoundaries[1].x) / 2;

        return playBoundaries;
    }

    LineRenderer SetLine(GameObject segment, Mesh mesh, Vector3 startPoint, Vector3 endPoint)
    {
        Vector3[] endpoints = new Vector3[] { startPoint, endPoint };
        LineRenderer line = segment.GetComponent<LineRenderer>();
        line.positionCount = endpoints.Length;
        line.SetPositions(endpoints);
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        line.BakeMesh(mesh, false);
        segment.GetComponent<MeshFilter>().mesh = mesh;

        segment.GetComponent<MeshCollider>().sharedMesh = mesh;

        segment.GetComponent<MeshRenderer>().enabled = false;

        return line;
    }
}


    // void FindBoundaries()
    // {
    //     width = (1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f) - offset) / 2;
    //     height = (1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f) - offset) / 2;


    //     topLeftCorner = new Vector2(-width, height);
    //     topRightCorner = new Vector2(width, height);
    //     bottomRightCorner = new Vector2(width, -height);
    //     bottomLeftCorner = new Vector2(-width, -height);
    // }