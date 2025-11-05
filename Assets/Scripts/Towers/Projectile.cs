using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected int damage;

    protected float speed;

    protected Transform target;

    private void FixedUpdate()
    {
        Move();
    }

    public virtual void Initialize(Transform newTarget, int damage, float speed)
    {
        target      = newTarget;
        this.damage = damage;
        this.speed  = speed;
    }

    protected abstract void Move();
    protected abstract void OnHit();
}