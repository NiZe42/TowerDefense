using UnityEngine;

public class AOEProjectile : Projectile
{
    [SerializeField]
    private float radius;

    [SerializeField]
    private LayerMask enemyLayer;

    protected override void Move()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime);

        if ((transform.position - target.position).sqrMagnitude <= float.Epsilon)
        {
            OnHit();
        }
    }

    protected override void OnHit()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, radius, enemyLayer))
        {
            Damageable enemy = GetEnemyDamageable(collider);
            enemy.ReceiveDamage(damage);
        }

        Destroy(gameObject);
    }

    private Damageable GetEnemyDamageable(Collider enemyChild)
    {
        Transform currentTransform = enemyChild.transform;

        while (currentTransform is not null)
        {
            var enemy = currentTransform.GetComponent<Damageable>();
            if (enemy is not null)
            {
                return enemy;
            }

            currentTransform = currentTransform.parent;
        }

        return null;
    }
}