using TMPro;
using UnityEngine;

/// <summary>
///     Responsible for keeping track of player money.
/// </summary>
public class PlayerMoneyUI : MonoBehaviour
{
    private TextMeshProUGUI moneyText;

    public void Awake()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
    }

    public void Start()
    {
        EventBus.Instance.Subscribe<OnPlayerMoneyChanged>(AdjustPlayerMoneyUI);
    }

    public void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<OnPlayerMoneyChanged>(AdjustPlayerMoneyUI);
        }
    }

    private void AdjustPlayerMoneyUI(OnPlayerMoneyChanged @event)
    {
        moneyText.text = $"Money: {@event.newMoney} coins";
    }

    public void SetPlayerMoneyText(int money)
    {
        moneyText.text = $"Money: {money} coins";
    }
}