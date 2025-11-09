using TMPro;
using UnityEngine;

/// <summary>
///     Responsible for keeping track of player health.
/// </summary>
public class PlayerHealthUI : MonoBehaviour
{
    private TextMeshProUGUI healthText;

    public void Awake()
    {
        healthText = GetComponent<TextMeshProUGUI>();
    }

    public void Start()
    {
        EventBus.Instance.Subscribe<OnPlayerHealthChanged>(AdjustPlayerHealthUI);
    }

    public void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<OnPlayerHealthChanged>(AdjustPlayerHealthUI);
        }
    }

    private void AdjustPlayerHealthUI(OnPlayerHealthChanged @event)
    {
        healthText.text = $"Health: {@event.newPlayerHealth}";
    }

    public void SetPlayerHealthText(int health)
    {
        healthText.text = $"Health: {health}";
    }
}