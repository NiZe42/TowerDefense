using System;
using UnityEngine;
using VolumetricLines;

/// <summary>
///     Shooting behavior, that applies slowing effect to a target.
/// </summary>
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

        lineInstance.StartPos = lineInstance.transform.InverseTransformPoint(firePoint.position);
        lineInstance.EndPos = lineInstance.transform.InverseTransformPoint(currentTarget.position);
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
        base.StopShooting();
        Debug.Log("Stopping slow effect");
        appliedSlow?.StopEffect();
        appliedSlow = null;

        lineInstance.gameObject.SetActive(false);
    }
}