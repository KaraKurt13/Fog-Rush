using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int MaxScore { get; set; }

    public int MaxTime { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsUnlocked { get; set; }

    public LevelData()
    {

    }

    public LevelData(int maxScore, int maxTime, bool isCompleted)
    {
        MaxScore = maxScore;
        MaxTime = maxTime;
        IsCompleted = isCompleted;
    }
}
