// PlayerEconomy.cs

using System;
using UnityEngine;

public class PlayerEconomyManager : MonoBehaviourSingleton<PlayerEconomyManager>, IEconomyValidator
{
    [SerializeField]
    private int money;

    private void Start()
    {
        EventBus.Instance.Subscribe<OnTowerBought>((Action<IEvent>)ProccessMoneyOperations);
        EventBus.Instance.Subscribe<OnTowerUpgraded>((Action<IEvent>)ProccessMoneyOperations);
        EventBus.Instance.Subscribe<OnTowerSold>((Action<IEvent>)ProccessMoneyOperations);
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
            default:
                return;
        }
    }
}