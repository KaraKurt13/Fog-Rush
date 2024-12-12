using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsTracker : MonoBehaviour
{
    private float _timeSpent = 0f;

    private bool _isTracking = false;

    private Player _relatedPlayer;

    void Update()
    {
        if (_isTracking)
            TrackStats();
    }

    public void Init(Player player, int health)
    {
        _relatedPlayer = player;
        Health = health;
        _maxHealth = health;
    }

    public void Reset()
    {
        _timeSpent = 0f;
        _isTracking = true;
        Health = _maxHealth;
        Coins = 0;
    }

    public void StartTracking()
    {
        _isTracking = true;
    }

    public void StopTracking()
    {
        _isTracking = false;
    }

    private void TrackStats()
    {
        _timeSpent += Time.deltaTime;
    }

    #region Health
    private int _maxHealth;

    public int Health { get; private set; } = 3;

    public void ReducePlayerHealth()
    {
        Health--;
        if (Health <= 0)
            Find.Engine.EndGame(_relatedPlayer, GameEndStatus.Lose);
    }

    public void IncreasePlayerHealth()
    {
        if (Health == _maxHealth) return;
        Health++;
    }
    #endregion Health

    #region Coins

    public int Coins { get; private set; } = 0;

    public void IncreaseCoin()
    {
        Coins++;
    }

    #endregion Coins

    public PlayerStats GetStats()
    {
        var stats = new PlayerStats(_timeSpent, 3, Coins);
        return stats;
    }
}
