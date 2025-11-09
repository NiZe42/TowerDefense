// PlayerEconomy.cs

using System;
using UnityEngine;

/// <summary>
///     Handles Player Economy. Controls cash flow of a player.
/// </summary>
public class PlayerEconomyManager : MonoBehaviourSingleton<PlayerEconomyManager>, IEconomyValidator
{
    [SerializeField]
    private int money;

    private void Start()
    {
        EventBus.Instance.Subscribe<OnTowerBought>((Action<IEvent>)ProccessMoneyOperations);
        EventBus.Instance.Subscribe<OnTowerUpgraded>((Action<IEvent>)ProccessMoneyOperations);
        EventBus.Instance.Subscribe<OnTowerSold>((Action<IEvent>)ProccessMoneyOperations);
        EventBus.Instance.Subscribe<OnEnemyDestroyed>((Action<IEvent>)ProccessMoneyOperations);

        UIManager.Instance.GetPlayerMoneyUI().SetPlayerMoneyText(money);
    }

    public override void OnDestroy()
    {
        if (EventBus.Instance is null)
        {
            return;
        }

        EventBus.Instance.Unsubscribe<OnTowerBought>((Action<IEvent>)ProccessMoneyOperations);
        EventBus.Instance.Unsubscribe<OnTowerUpgraded>((Action<IEvent>)ProccessMoneyOperations);
        EventBus.Instance.Unsubscribe<OnTowerSold>((Action<IEvent>)ProccessMoneyOperations);
        EventBus.Instance.Unsubscribe<OnEnemyDestroyed>((Action<IEvent>)ProccessMoneyOperations);

        base.OnDestroy();
    }

    public bool CanAfford(int amount)
    {
        return money >= amount;
    }

    public void Spend(int amount)
    {
        money -= amount;
    }

    public void Earn(int amount)
    {
        money += amount;
    }

    private void ProccessMoneyOperations(IEvent @event)
    {
        switch (@event)
        {
            case OnTowerBought towerBought:
                Spend(towerBought.cost);
                break;
            case OnTowerUpgraded towerUpgraded:
                Spend(towerUpgraded.cost);
                break;
            case OnTowerSold towerSold:
                Earn(towerSold.moneyGained);
                break;
            case OnEnemyDestroyed enemyDestroyed:
                Earn(enemyDestroyed.droppedMoney);
                break;
        }

        EventBus.Instance.InvokeEvent(new OnPlayerMoneyChanged { newMoney = money });
    }
}