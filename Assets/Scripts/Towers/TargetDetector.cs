using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    [SerializeField]
    private SphereCollider sphereCollider;

    [HideInInspector]
    public List<Transform> enemiesInRange = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Enemy enemy))
        {
            return;
        }

        enemiesInRange.Add(enemy.transform);
        OnEnemyEnteredRange?.Invoke(enemy.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Enemy enemy))
        {
            return;
        }

        enemiesInRange.Remove(enemy.transform);
        OnEnemyExitedRange?.Invoke(enemy.transform);
    }

    public event Action<Transform> OnEnemyEnteredRange;
    public event Action<Transform> OnEnemyExitedRange;

    public SphereCollider GetSphereCollider()
    {
        return sphereCollider;
    }
}