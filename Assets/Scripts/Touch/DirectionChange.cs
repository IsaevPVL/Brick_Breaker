using UnityEngine;

public class DirectionChange : PlaceableObject
{
    Ball ball;
    Vector3 desiredDirection;
    bool primed;

    private void Update()
    {
        if (!isTouched)
        {   
            if(primed){
                ball.GetComponent<Rigidbody>().velocity = desiredDirection.normalized * ball.speed;
                primed = false;
            }
            return;
        }
        primed = true;
        desiredDirection = touchPosition;
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        Vector3 ballOffset = ball.transform.position - objectPositionAtTouch + objectTouchOffset;
        desiredDirection = (desiredDirection + ballOffset) - ball.transform.position;
    }
}