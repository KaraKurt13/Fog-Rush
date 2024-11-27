using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsTracker : MonoBehaviour
{
    private float _timeSpent = 0f;

    private bool _isTracking = false;

    void Update()
    {
        if (_isTracking)
            TrackStats();
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

    public PlayerStats GetStats()
    {
        var stats = new PlayerStats(_timeSpent, 3);
        return stats;
    }
}
