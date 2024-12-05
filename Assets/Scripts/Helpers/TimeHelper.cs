using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeHelper
{
    public const int TicksPerSecond = 50;

    public static int SecondsToTicks(float seconds)
    {
        return Mathf.CeilToInt(seconds * TicksPerSecond);
    }
}
