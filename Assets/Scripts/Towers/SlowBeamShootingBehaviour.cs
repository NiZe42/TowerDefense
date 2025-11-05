using System;
using UnityEngine;
using VolumetricLines;

[Serializable]
public class SlowBeamShootingBehaviour : ShootingBehaviour
{
    [SerializeField]
    private float slowMultiplier = 0.5f;

    [SerializeField]
    private VolumetricLineBehavior lineBehaviourPrefab;

    private SlowEffect appliedSlow;

    private bool isShooting;
    private VolumetricLineBehavior lineInstance;

    private void Awake()
    {
        lineInstance = Instantiate(
            lineBehaviourPrefab,
            firePoint.position,
            Quaternion.identity,
            transform);

        lineInstance.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (!target)
        {
            return;
        }

        if (!isShooting)
        {
            return;
        }

        lineInstance.StartPos = firePoint.position;
        lineInstance.EndPos   = target.position;
    }

    public override void StartShooting(Transform newTarget)
    {
        base.StartShooting(newTarget);

        isShooting = true;

        if (!target.TryGetComponent(out Enemy enemy))
        {
            return;
        }

        appliedSlow = new SlowEffect(
            enemy,
            slowMultiplier,
            Mathf.Infinity,
            0f);

        appliedSlow.Apply();
        lineInstance.gameObject.SetActive(true);
    }

    public override void StopShooting()
    {
        appliedSlow?.StopEffect();
        appliedSlow = null;
        isShooting  = false;

        lineInstance.gameObject.SetActive(false);
    }
}