using System;
using UnityEngine;

[Serializable]
public class ProjectileShootingBehaviour : ShootingBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float cooldown;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float projectileSpeed;

    private bool canShoot;

    private float nextShotTime;

    public override void Update()
    {
        if (canShoot && Time.time >= nextShotTime)
        {
            Shoot();
        }
    }

    public override void StartShooting(Transform newTarget)
    {
        base.StartShooting(newTarget);
        canShoot = true;
    }

    public override void StopShooting()
    {
        canShoot = false;
    }

    private void Shoot()
    {
        new ProjectileFactory().FromPrefab(projectilePrefab).AtPosition(firePoint.position)
            .AtTarget(target).WithDamage(damage).WithSpeed(projectileSpeed).Build();

        Debug.Log("Projectile shot!");
        nextShotTime = Time.time + cooldown;
    }
}