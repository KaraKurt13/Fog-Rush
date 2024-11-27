using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public float TimeSpent { get; }

    public int RemainingHealth { get; }

    public PlayerStats(float timeSpent, int remainingHealth)
    {
        TimeSpent = timeSpent;
        RemainingHealth = remainingHealth;
    }
}
