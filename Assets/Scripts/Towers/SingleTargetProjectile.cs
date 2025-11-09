using UnityEngine;

/// <summary>
///     Projectile type, that deals damage only to selected target.
/// </summary>
public class SingleTargetProjectile : Projectile
{
    private float rotateSpeed;

    public override void Initialize(Transform newTarget, int damage, float speed)
    {
        base.Initialize(newTarget, damage, speed);
        rotateSpeed = speed * 5f;
    }

    protected override void Move()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }

        Vector3    direction      = (target.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime);

        transform.position += speed * Time.deltaTime * transform.forward;

        if ((transform.position - target.position).sqrMagnitude <= .05f)
        {
            Debug.Log("reached it");
            OnHit();
        }
    }

    protected override void OnHit()
    {
        if (target.gameObject.TryGetComponent(out Damageable dmg))
        {
            dmg.ReceiveDamage(damage);
        }
        else
        {
            Debug.LogWarning($"Target {target.name} has no Damageable component!");
        }

        Destroy(gameObject);
    }
}