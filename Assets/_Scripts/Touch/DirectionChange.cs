using UnityEngine;

public class DirectionChange : PlaceableObject
{
    Ball ball;
    Vector3 desiredDirection;
    bool primed;

    //DEAL WITH OVERRIDE
    public override void Update()
    {

        if (!isTouched)
        {   
            if(primed){
                EnergyManager.active.UseEnergyBars(1);
                ball.rb.velocity = desiredDirection.normalized * ball.speed;
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
        //ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        ball = GameObject.FindObjectOfType<Ball>();
        Vector3 ballOffset = ball.transform.position - objectPositionAtTouch + objectTouchOffset;
        desiredDirection = (desiredDirection + ballOffset) - ball.transform.position;
    }
}