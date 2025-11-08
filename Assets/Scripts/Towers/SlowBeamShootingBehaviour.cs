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

    private VolumetricLineBehavior lineInstance;

    private void Awake()
    {
        lineInstance = Instantiate(
            lineBehaviourPrefab,
            transform.position,
            Quaternion.identity,
            transform).GetComponent<VolumetricLineBehavior>();

        lineInstance.gameObject.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (!currentTarget)
        {
            return;
        }

        if (!isShooting)
        {
            return;
        }

        lineInstance.StartPos = firePoint.position;
        lineInstance.EndPos   = currentTarget.position;
    }

    public override void StartShooting()
    {
        base.StartShooting();

        if (!currentTarget.TryGetComponent(out Enemy enemy))
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