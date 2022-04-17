using UnityEngine;
using System;

public class Hitter : MonoBehaviour
{
    public static event Action<int> DamageTaken;

    public void TakeDamage(int amount)
    {
        DamageTaken?.Invoke(amount);
    }
}