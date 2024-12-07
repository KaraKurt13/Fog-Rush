using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public float TimeSpent { get; }

    public int RemainingHealth { get; }
    
    public int Coins { get; }

    public PlayerStats(float timeSpent, int remainingHealth, int coinsAmount)
    {
        TimeSpent = timeSpent;
        RemainingHealth = remainingHealth;
        Coins = coinsAmount;
    }
}
