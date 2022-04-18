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
                EnergyManager.active.UseEnergyBars(1);
                ball.GetComponent<Rigidbody>().velocity = desiredDirection.normalized * ball.speed;
                primed = false;
            }
            return;
        }

        if(!EnergyManager.active.CheckIfEnergyBarsAvailable(1)){
            Debug.Log("Not enough energy");
            return;
        }
        primed = true;
        desiredDirection = touchPosition;
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        Vector3 ballOffset = ball.transform.position - objectPositionAtTouch + objectTouchOffset;
        desiredDirection = (desiredDirection + ballOffset) - ball.transform.position;
    }
}