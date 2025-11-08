using System;
using UnityEngine;

[Serializable]
public abstract class ShootingBehaviour : MonoBehaviour
{
    [SerializeField]
    protected TargetDetector targetDetector;

    protected float attackCooldown;

    protected Transform currentTarget;
    protected int damage;
    protected Transform firePoint;

    protected bool isShooting;

    protected float range;

    public virtual void Update()
    {
        if (currentTarget is null)
        {
            TrySelectTarget();
        }

        if (currentTarget is not null)
        {
            if (!isShooting)
            {
                StartShooting();
            }
        }
        else
        {
            if (isShooting)
            {
                StopShooting();
            }
        }
    }

    public void OnDestroy()
    {
        targetDetector.OnEnemyEnteredRange -= EnemyEnteredRange;
        targetDetector.OnEnemyExitedRange  -= EnemyExitedRange;
    }

    public void Initialize(
        int damage,
        Transform firePoint,
        float range,
        float attackCooldown)
    {
        this.damage         = damage;
        this.firePoint      = firePoint;
        this.range          = range;
        this.attackCooldown = attackCooldown;

        SphereCollider sphere = targetDetector.GetSphereCollider();
        sphere.radius = range;

        targetDetector.OnEnemyEnteredRange += EnemyEnteredRange;
        targetDetector.OnEnemyExitedRange  += EnemyExitedRange;
    }

    public void EnemyEnteredRange(Transform target) { }

    public void EnemyExitedRange(Transform target)
    {
        if (currentTarget == target)
        {
            currentTarget = null;
        }
    }

    public void TrySelectTarget()
    {
        if (targetDetector.enemiesInRange.Count > 0)
        {
            currentTarget = targetDetector.enemiesInRange[0];
        }
    }

    public virtual void StartShooting() { isShooting = true; }

    public virtual void StopShooting() { isShooting = false; }

    public virtual void Initialize(Transform firePoint)
    {
        this.firePoint = firePoint;
    }
}