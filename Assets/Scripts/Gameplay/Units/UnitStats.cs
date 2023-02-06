using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitStats
{
    [Header("Stats")]
    public float Health;
    public float Power;
    public float MaxHealth;

    [Header("Statuses")]
    public bool IsDead;
    public bool IsGrabbed;
    public bool IsLocked => _lockTimes.Count > 0;

    private readonly List<float> _lockTimes = new List<float>();

    public void AddLock(float time)
    {
        _lockTimes.Add(time);
    }

    public void ReduceTimers()
    {
        for (int i = _lockTimes.Count - 1; i >= 0; i--)
        {
            if (_lockTimes[i] > 0)
            {
                _lockTimes[i] -= Time.deltaTime;
            }
            else
            {
                _lockTimes.RemoveAt(i);
            }
        }
    }
}
