using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealthGain : PlaceableObject
{
    [SerializeField] int healthToGain;

    // public override void OnEnable()
    // {
    //     base.OnEnable();
    // }

    // public override void OnDisable()
    // {
    //     base.OnDisable();
    // }

    protected override void SubscribeToConnection(PlaceableObject obj)
    {
        obj.ProgramTriggered += WaitForHit;
        print(this.name + $" subscribed to {obj.name}");
    }

    protected override void UnsubscribeFromConnection(PlaceableObject obj)
    {
        obj.ProgramTriggered -= WaitForHit;
        print(this.name + $" unsubscribed from {obj.name}");
    }

    void WaitForHit()
    {
        Ball.BrickHit += GainHealth;
    }

    void GainHealth(GameObject objectHit)
    {
        HealthManager.active.AddHealth(healthToGain);
        print("Health gained");
        Ball.BrickHit -= GainHealth;
    }


    // protected override void SubscribeToConnections()
    // {
    //     foreach (PlaceableObject obj in objectsConnectedTo)
    //     {
    //         obj.ProgramTriggered += WaitForHit;
    //     }
    // }

}
