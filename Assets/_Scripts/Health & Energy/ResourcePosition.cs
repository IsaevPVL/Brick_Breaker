using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResourcePosition : MonoBehaviour
{
    [SerializeField] float maxXOffset;
    [SerializeField] float minXOffset;
    [SerializeField] float yOffset;
    [SerializeField] float zOffset;
    [SerializeField] float followTime = 0.05f;
    [SerializeField] Ease yEaseType;
    [SerializeField] Ease followEaseType;
    Transform paddle;
    float maxPaddleXPosition;
    Boundaries boundaries;
    //float fieldHalfWidth;

    float halfWidth;
    float minXPosition;
    float maxXPosition;
    Vector3 desiredPosition;
    int flip;

    GameObject paddleObj; //TEMPORARY, REMOVE FROM UPDATE

    private void Start() {
        paddleObj = GameObject.FindGameObjectWithTag("Paddle");
        paddle = paddleObj.GetComponent<Transform>();
        maxPaddleXPosition = paddleObj.GetComponent<PaddleControl>().maxAllowedWidth;
        
        boundaries = GameObject.FindObjectOfType<Boundaries>();
        //fieldHalfWidth = boundaries.corners[1].x;

        halfWidth = GetComponent<BoxCollider>().size.x / 2;
        maxXPosition = boundaries.corners[1].x - halfWidth + maxXOffset;
        //Debug.Log(maxXPosition);
        minXPosition = maxXPosition + minXOffset;
        //Debug.Log(minXPosition);

        desiredPosition.y = paddle.position.y + yOffset;
        desiredPosition.z = paddle.position.z + zOffset;
        
        //transform.DOShakePosition(1, 0.05f, 5, 180, false, false).SetLoops(-1);
        transform.DOMoveY(desiredPosition.y - 0.2f, 1).SetLoops(-1, LoopType.Yoyo).SetEase(yEaseType);
    }

    private void Update() {
        // desiredPosition.x = -paddle.position.x;
        if(paddle.position.x > 0.1){
            flip = 1;
        }else{
            flip = -1;
        }

        float paddlePositionX = Mathf.InverseLerp(0, paddleObj.GetComponent<PaddleControl>().maxAllowedWidth, paddle.position.x * flip);
        //Debug.Log(paddle.position.x + " : " + paddlePositionX);
        desiredPosition.x = Mathf.Lerp(maxXPosition, minXPosition,  paddlePositionX) * -flip; 
        //transform.position = desiredPosition;
        transform.DOMoveX(desiredPosition.x, followTime).SetEase(followEaseType);
    }
}
