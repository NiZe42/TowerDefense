using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private GenericSlider healthSlider;

    [SerializeField]
    private Damageable damageable;

    [SerializeField]
    private float offsetFromOrigin;

    private Vector3 localOrigin;

    private Camera mainCamera;

    private void Start()
    {
        localOrigin              =  transform.localPosition;
        mainCamera               =  Camera.main;
        damageable.OnDamageTaken += UpdateBar;
        UpdateBar(damageable.Health, damageable.MaxHealth);
    }

    private void LateUpdate()
    {
        Vector3 direction = (mainCamera.transform.position - transform.position).normalized;
        /*
        Vector3 localDirection = transform.parent.InverseTransformDirection(direction);
        transform.localPosition = localOrigin -
                    new Vector3(localDirection.x, 0f, localDirection.z) * offsetFromOrigin;
        */
        transform.rotation = Quaternion.LookRotation(new Vector3(0, direction.y, 0f));
    }

    private void OnDestroy()
    {
        damageable.OnDamageTaken += UpdateBar;
    }

    private void UpdateBar(int newHealth, int maxHealth)
    {
        float normalizedHealth = newHealth / (float) maxHealth;
        healthSlider.SetTargetValue(normalizedHealth);
    }
}