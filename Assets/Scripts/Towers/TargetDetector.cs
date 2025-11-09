using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Script, that keep track of every Enemy inside the collider
/// </summary>
public class TargetDetector : MonoBehaviour
{
    [SerializeField]
    private SphereCollider sphereCollider;

    [HideInInspector]
    public List<Transform> enemiesInRange = new List<Transform>();

    private void Awake() { }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        var enemy = other.GetComponentInParent<Enemy>();
        if (enemy is null)
        {
            return;
        }

        enemiesInRange.Add(enemy.transform);
        OnEnemyEnteredRange?.Invoke(enemy.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponentInParent<Enemy>();
        if (enemy is null)
        {
            return;
        }

        enemiesInRange.Remove(enemy.transform);
        OnEnemyExitedRange?.Invoke(enemy.transform);
    }

    public void Initialize(float radius)
    {
        sphereCollider.radius = radius;

        Vector3 center = transform.position + sphereCollider.center;

        Collider[] hits = Physics.OverlapSphere(center, sphereCollider.radius);

        foreach (Collider hit in hits)
        {
            var enemy = hit.gameObject.GetComponentInParent<Enemy>();
            if (enemy is null)
            {
                return;
            }

            if (enemiesInRange.Contains(enemy.transform))
            {
                continue;
            }

            enemiesInRange.Add(enemy.transform);
        }
    }

    public event Action<Transform> OnEnemyEnteredRange;
    public event Action<Transform> OnEnemyExitedRange;

    public SphereCollider GetSphereCollider()
    {
        return sphereCollider;
    }
}