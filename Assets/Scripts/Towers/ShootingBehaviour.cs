using System;
using UnityEngine;

/// <summary>
///     Base abstract class that defines a contract of how shooting should work.
/// </summary>
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
        if (currentTarget == null)
        {
            if (isShooting)
            {
                StopShooting();
            }

            TrySelectTarget();
        }

        if (currentTarget != null && !isShooting)
        {
            StartShooting();
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

        targetDetector.Initialize(this.range);

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
        targetDetector.enemiesInRange.RemoveAll(target => target == null);

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