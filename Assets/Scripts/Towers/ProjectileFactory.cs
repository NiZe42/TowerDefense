using UnityEngine;

public class ProjectileFactory
{
    private int damage;
    private GameObject prefab;
    private Vector3 spawnPos;
    private Quaternion spawnRotation;
    private float speed;
    private Transform target;

    public ProjectileFactory FromPrefab(GameObject prefab)
    {
        this.prefab = prefab;
        return this;
    }

    public ProjectileFactory AtPosition(Vector3 pos)
    {
        spawnPos = pos;
        return this;
    }

    public ProjectileFactory AtTarget(Transform target)
    {
        this.target = target;
        return this;
    }

    public ProjectileFactory WithRotation(Quaternion rotation)
    {
        spawnRotation = rotation;
        return this;
    }

    public ProjectileFactory WithDamage(int damage)
    {
        this.damage = damage;
        return this;
    }

    public ProjectileFactory WithSpeed(float speed)
    {
        this.speed = speed;
        return this;
    }

    public Projectile Build()
    {
        GameObject instance   = Object.Instantiate(prefab, spawnPos, spawnRotation);
        var        projectile = instance.GetComponent<Projectile>();

        if (projectile is null)
        {
            Debug.Log(
                $"ProjectileFactory: Prefab {prefab.name} does not contain a Projectile component!");

            Object.Destroy(instance);
            return null;
        }

        projectile.Initialize(target, damage, speed);
        return projectile;
    }
}