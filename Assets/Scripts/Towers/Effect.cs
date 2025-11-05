using System;
using UnityEngine;

public interface IEffect
{
    Type TargetType { get; }
    void Apply();
    void StopEffect();

    void Tick();
    event Action<IEffect> OnCompleted;
}

public abstract class Effect<TTarget> : IEffect where TTarget : MonoBehaviour
{
    protected readonly float durationSeconds;
    protected readonly float intervalSeconds;
    protected readonly TTarget target;

    protected IntervalTimer timer;

    public Effect(TTarget target, float durationSeconds, float intervalSeconds)
    {
        this.target          =  target;
        this.durationSeconds =  durationSeconds;
        this.intervalSeconds =  intervalSeconds;
        timer                =  new IntervalTimer(durationSeconds, intervalSeconds);
        timer.OnEnd          += StopEffect;
    }

    public Type TargetType => typeof(TTarget);

    public virtual void Apply()
    {
        var effectController = target.gameObject.GetComponent<EffectController>();
        if (effectController is null)
        {
            Debug.Log("Could not find effectController on affected object");
            return;
        }

        effectController.AddEffect(this);

        timer.StartTimer();
    }

    public virtual void StopEffect()
    {
        timer.StopTimer();
        timer = null;
        OnCompleted?.Invoke(this);
    }

    public virtual void Tick()
    {
        timer.Tick();
    }

    public event Action<IEffect> OnCompleted;
}