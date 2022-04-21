using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    //Non-allocating WaitForSeconds
    public static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float seconds){
        if(WaitDictionary.TryGetValue(seconds, out WaitForSeconds wait)){
            return wait;
        }
        WaitDictionary[seconds] = new WaitForSeconds(seconds);
        return WaitDictionary[seconds];
    }
}
