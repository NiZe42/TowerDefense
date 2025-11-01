using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] 
    private GenericSlider healthSlider;
    [SerializeField] 
    private Damageable damageable;
    
    private void Start() {
        damageable.OnDamageTaken += UpdateBar;
        UpdateBar(damageable.Health, damageable.MaxHealth);
    }
    
    private void OnDestroy() {
        damageable.OnDamageTaken += UpdateBar;
    }
    
    private void UpdateBar(int newHealth, int maxHealth)
    {
        float normalizedHealth = (float)newHealth / (float)maxHealth;
        healthSlider.SetTargetValue(normalizedHealth);
    }

    private void LateUpdate()
    {
        //transform.LookAt(Camera.main.transform); // face the camera
    }
}
