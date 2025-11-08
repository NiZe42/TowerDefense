using System;
using UnityEngine;

[Serializable]
public class ProjectileShootingBehaviour : ShootingBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float projectileSpeed;

    private float nextShotTime;

    public override void Update()
    {
        base.Update();
        if (isShooting && Time.time >= nextShotTime)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3    direction = (currentTarget.position - firePoint.position).normalized;
        Quaternion rotation  = Quaternion.LookRotation(direction);

        new ProjectileFactory().FromPrefab(projectilePrefab).AtPosition(firePoint.position)
            .WithRotation(rotation).AtTarget(currentTarget).WithDamage(damage)
            .WithSpeed(projectileSpeed).Build();

        Debug.Log("Projectile shot!");
        nextShotTime = Time.time + attackCooldown;
    }
}