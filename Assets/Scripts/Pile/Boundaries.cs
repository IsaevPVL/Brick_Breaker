using UnityEngine;

public class Boundaries : MonoBehaviour
{
    Camera cam;
    public float width;
    public float height;

    public float offset = 0.1f;
    public float lineWidth = 0.05f;

    float zOffset;
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
    public Vector3[] deeperCorners = new Vector3[4];

    private void Awake()
    {
        cam = Camera.main;
        
        //Visible border
        float distanceToCamera = GameObject.FindGameObjectWithTag("Managers").transform.position.z - cam.transform.position.z;
        FindScreenBoundaries(distanceToCamera);
        corners = GetPlayBoundaries();
        
        //Colliders on paddle/bricks plane
        zOffset = GameObject.FindGameObjectWithTag("Paddle").transform.position.z - cam.transform.position.z;
        FindScreenBoundaries(zOffset);
        deeperCorners = GetPlayBoundaries();
        float zWidth = deeperCorners[0].x;
        float zHeight = deeperCorners[0].y;
        float zDepth = deeperCorners[0].z;
        SetCollider(top, new Vector3(0, zHeight, zDepth), new Vector3(Mathf.Abs(zWidth) * 2f, 0, 1));
        SetCollider(left, new Vector3(zWidth, 0, zDepth), new Vector3(0, Mathf.Abs(zHeight) * 2f, 1));
        SetCollider(bottom, new Vector3(0, -zHeight, zDepth), new Vector3(Mathf.Abs(zWidth) * 2f, 0, 1));
        SetCollider(right, new Vector3(-zWidth, 0, zDepth), new Vector3(0, Mathf.Abs(zHeight) * 2f, 1));

        //0 = Top Left, clockwise
        SetLine(top, corners[0], corners[1]);
        //top.transform.position = new Vector3(top.transform.position.x, height, -height + zOffset);

        SetLine(right, corners[1], corners[2]);
        //right.transform.position = new Vector3(-width, right.transform.position.y, -width + zOffset);

        LineRenderer bottomLine = SetLine(bottom, corners[2], corners[3]);
        //bottom.transform.position = new Vector3(bottom.transform.position.x, -height, height + zOffset);
        // + 10f temporary value to offset from the bottom
        //bottomLine.startColor = Color.red;
        //bottomLine.endColor = Color.red;

        SetLine(left, corners[3], corners[0]);
        //left.transform.position = new Vector3(width, left.transform.position.y, -width + zOffset);

    }

    void FindScreenBoundaries(float zDistance)
    {

        topLeftCorner = cam.ViewportToWorldPoint(new Vector3(0, 1, zDistance));


        topRightCorner = cam.ViewportToWorldPoint(new Vector3(1, 1, zDistance));
        bottomRightCorner = cam.ViewportToWorldPoint(new Vector3(1, 0, zDistance));
        bottomLeftCorner = cam.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
    }

    Vector3[] GetPlayBoundaries(){
        Vector3[] playBoundaries = new Vector3[4];

        float screenHeight = topLeftCorner.y - bottomLeftCorner.y;
        float screenWidth = topLeftCorner.x - topRightCorner.x;

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

    LineRenderer SetLine(GameObject segment, Vector3 startPoint, Vector3 endPoint)
    {
        Vector3[] endpoints = new Vector3[] { startPoint, endPoint };
        LineRenderer line = segment.GetComponent<LineRenderer>();
        line.positionCount = endpoints.Length;
        line.SetPositions(endpoints);
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        // line.BakeMesh(mesh, true);
        // segment.GetComponent<MeshFilter>().mesh = mesh;

        // segment.GetComponent<MeshCollider>().sharedMesh = mesh;

        // segment.GetComponent<MeshRenderer>().enabled = false;

        return line;
    }

    void SetCollider(GameObject segment, Vector3 position, Vector3 size){
        BoxCollider collider = segment.GetComponent<BoxCollider>();
        collider.center = position;
        collider.size = size;
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