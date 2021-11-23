using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    Camera cam;
    float width;
    float height;

    public float offset = 0.1f;
    public float lineWidth = 0.05f;

    EdgeCollider2D edge;
    LineRenderer line;

    //public GameObject[] boundaries = new GameObject[4];

    public GameObject top;
    public GameObject right;
    public GameObject bottom;
    public GameObject left;

    public GameObject visibleTop;
    public GameObject visibleRight;
    public GameObject visibleBottom;
    public GameObject visibleLeft;

    Vector3 topLeftCorner;
    Vector3 topRightCorner;
    Vector3 bottomRightCorner;
    Vector3 bottomLeftCorner;

    private void Awake() {
        cam = Camera.main;


    }

    private void Start() {

    }

    void Update()
    {
        FindBoundaries();
        //SetBoundaries();
        SetLine(top, visibleTop, topLeftCorner, topRightCorner);
        SetLine(right, visibleRight, topRightCorner, bottomRightCorner);
        SetLine(bottom, visibleBottom, bottomRightCorner, bottomLeftCorner);
        SetLine(left, visibleLeft, bottomLeftCorner, topLeftCorner);

    }

    void SetLine(GameObject position, GameObject visiblePosition, Vector3 startPoint, Vector3 endPoint){
        Vector3[] endpoints = new Vector3[] {startPoint, endPoint};
        LineRenderer line = position.GetComponent<LineRenderer>();
        line.positionCount = endpoints.Length;
        line.SetPositions(endpoints);
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        LineRenderer lineVisible = visiblePosition.GetComponent<LineRenderer>();
        lineVisible.positionCount = endpoints.Length;
        lineVisible.SetPositions(endpoints);
        lineVisible.startWidth = lineWidth;
        lineVisible.endWidth = lineWidth;
    }
    

    void FindBoundaries(){
        width = 1/(cam.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f) - offset;
        height = 1/(cam.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f) - offset;

        topLeftCorner = new Vector2(-width / 2, height / 2);
        topRightCorner = new Vector2(width / 2, height / 2);
        bottomRightCorner = new Vector2(width / 2, -height / 2);
        bottomLeftCorner = new Vector2(-width / 2, -height / 2);
    }

    void SetBoundaries(){
        Vector2 pointA = new Vector2(-width / 2, -height / 2);
        Vector2 pointB = new Vector2(-width / 2, height / 2);
        Vector2 pointC = new Vector2(width / 2, height / 2);
        Vector2 pointD = new Vector2(width / 2, -height / 2);
        Vector2[] pointArray = new Vector2[] {pointA, pointB, pointC, pointD, pointA};
        edge.points = pointArray;

        line.positionCount = pointArray.Length;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        Vector3[] positions = new Vector3[] {pointA, pointB, pointC, pointD, pointA};
        line.SetPositions(positions);
    }

    void DrawLine(){
        
    }
}
