using UnityEngine;
using System;

public class DirectionChange : PlaceableObject
{
    Ball ball;
    Vector3 desiredDirection;
    bool primed;
    bool useAttempted;

    public override event Action ProgramTriggered;

    public override void Update()
    {

        if (isUnlocked)
        {
            base.Update();
        }
        else
        {

            if (!isTouched)
            {
                useAttempted = false;

                if (primed)
                {
                    EnergyManager.active.UseEnergyBars(1);
                    //ball.rb.velocity = desiredDirection.normalized * ball.speed;
                    ball.SetDirection(desiredDirection);
                    primed = false;

                    ProgramTriggered?.Invoke();
                }
                return;
            }

            if(useAttempted){
                return;
            }

            if (!EnergyManager.active.CheckIfEnergyBarsAvailable(1))
            {
                Debug.Log("Not enough energy");
                useAttempted = true;
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
}