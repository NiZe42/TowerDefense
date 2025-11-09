using System;
using UnityEngine;

// Would like to do bootstraping magic later to make it so Tick is called automatically by engine.
/// <summary>
///     Timer with events OnInterval and Ontick, that allow for generic use of overtime entities.
/// </summary>
public class IntervalTimer
{
    private readonly float intervalSeconds;
    private readonly float totalSeconds;

    private bool isRunning;
    private float lastIntervalTime;

    private float originalStartTime;

    public IntervalTimer(float totalSeconds, float intervalSeconds)
    {
        this.intervalSeconds = intervalSeconds;
        this.totalSeconds    = totalSeconds;
    }

    public void Tick()
    {
        if (!isRunning)
        {
            return;
        }

        if (Time.time >= originalStartTime + totalSeconds)
        {
            OnEnd?.Invoke();
            return;
        }

        {
            if (!(Time.time >= lastIntervalTime + intervalSeconds))
            {
                return;
            }
        }

        OnInterval?.Invoke();
        lastIntervalTime = Time.time;
    }

    public event Action OnInterval;
    public event Action OnEnd;

    public void StartTimer()
    {
        originalStartTime = Time.time;
        lastIntervalTime  = Time.time - intervalSeconds - 1;
        isRunning         = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        lastIntervalTime  = Time.time - intervalSeconds - 1;
        originalStartTime = Time.time;
    }
}