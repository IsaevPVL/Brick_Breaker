using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    //Non-allocating WaitForSeconds
    public static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float seconds)
    {
        if (WaitDictionary.TryGetValue(seconds, out WaitForSeconds wait))
        {
            return wait;
        }
        WaitDictionary[seconds] = new WaitForSeconds(seconds);
        return WaitDictionary[seconds];
    }

    //And WaitForSecondsRealtime
    public static readonly Dictionary<float, WaitForSecondsRealtime> WaitRealtimeDictionary = new Dictionary<float, WaitForSecondsRealtime>();
    public static WaitForSecondsRealtime GetWaitRealtime(float seconds)
    {
        if (WaitRealtimeDictionary.TryGetValue(seconds, out WaitForSecondsRealtime wait))
        {
            return wait;
        }
        WaitRealtimeDictionary[seconds] = new WaitForSecondsRealtime(seconds);
        return WaitRealtimeDictionary[seconds];
    }
}
