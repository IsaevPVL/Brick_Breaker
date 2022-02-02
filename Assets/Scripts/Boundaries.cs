using UnityEngine;

public class Boundaries : MonoBehaviour
{
    Camera cam;
    float width;
    float height;

    public float offset = 0.1f;
    public float lineWidth = 0.05f;

    public GameObject top;
    public GameObject right;
    public GameObject bottom;
    public GameObject left;

    Vector3 topLeftCorner;
    Vector3 topRightCorner;
    Vector3 bottomRightCorner;
    Vector3 bottomLeftCorner;

    private void Awake()
    {
        cam = Camera.main;
        
        FindBoundaries();
        SetLine(top, new Mesh(), topLeftCorner, topRightCorner);
        top.transform.position = new Vector3(top.transform.position.x, height, height);

        SetLine(right, new Mesh(), topRightCorner, bottomRightCorner);
        right.transform.position = new Vector3(width, right.transform.position.y, width);

        LineRenderer bottomLine = SetLine(bottom, new Mesh(), bottomRightCorner, bottomLeftCorner);
        bottom.transform.position = new Vector3(bottom.transform.position.x, -height + 11.6f, height - 11.6f);
        //4.6 temporary value
        bottomLine.startColor = Color.red;
        bottomLine.endColor = Color.red;

        SetLine(left, new Mesh(), bottomLeftCorner, topLeftCorner);
        left.transform.position = new Vector3(-width, left.transform.position.y, width);
    }

    private void Start()
    {
    }

    void Update()
    {
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

    void FindBoundaries()
    {
        width = (1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f) - offset) / 2;
        height = (1 / (cam.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f) - offset) / 2;


        topLeftCorner = new Vector2(-width, height);
        topRightCorner = new Vector2(width, height);
        bottomRightCorner = new Vector2(width, -height + 11.6f);
        bottomLeftCorner = new Vector2(-width, -height + 11.6f);
    }

    void FindBoundaries2(){

    }
}