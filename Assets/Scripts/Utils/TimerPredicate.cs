using UnityEngine;

/// <summary>
///     Predicates, that evaluates to true after specific time passes.
/// </summary>
public class TimerPredicate : IPredicate
{
    private readonly float duration;
    private float startTime;

    public TimerPredicate(float durationSeconds)
    {
        duration  = durationSeconds;
        startTime = Time.time;
    }

    public bool Evaluate()
    {
        return Time.time >= startTime + duration;
    }

    public void OnEnter()
    {
        startTime = Time.time;
    }
}