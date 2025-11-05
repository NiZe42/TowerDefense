using System;
using UnityEngine;

[Serializable]
public abstract class ShootingBehaviour : MonoBehaviour
{
    protected Transform firePoint;
    protected Transform target;
    public abstract void Update();

    public virtual void StartShooting(Transform target)
    {
        this.target = target;
    }

    public abstract void StopShooting();

    public virtual void Initialize(Transform firePoint)
    {
        this.firePoint = firePoint;
    }
}