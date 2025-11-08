using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviourSingleton<TowerManager>
{
    public TowerUpgradeDatabaseSO towerUpgradeDatabase;

    public IEconomyValidator economyValidator;

    private int nextTowerId;
    public Dictionary<int, Tower> towers { get; } = new Dictionary<int, Tower>();

    private EventBus eventBusInstance => EventBus.Instance;

    public override void Awake()
    {
        towerUpgradeDatabase.Initialize();
        base.Awake();
    }

    public override void OnDestroy()
    {
        towers.Clear();
        base.OnDestroy();
    }

    private bool PlayerHasMoneyFor(TowerLevelDataSO towerLevelData)
    {
        return economyValidator.CanAfford(towerLevelData.cost);
    }

    public void BuildTower(Vector3 position, TowerLevelDataSO towerLevelData)
    {
        if (!PlayerHasMoneyFor(towerLevelData))
        {
            return;
        }

        var tower = Instantiate(towerLevelData.towerPrefab, position, Quaternion.identity)
            .gameObject.GetComponent<Tower>();

        int id = nextTowerId++;
        tower.Initialize(towerLevelData, id);

        towers.Add(id, tower);
        TileGrid.Instance.ChangeBlockOccupancy(position, true);

        eventBusInstance.InvokeEvent(new OnTowerBought { cost = towerLevelData.cost });
    }

    public void UpgradeTower(int id, TowerLevelDataSO newTowerLevelData)
    {
        if (!towers.TryGetValue(id, out Tower tower))
        {
            return;
        }

        if (!PlayerHasMoneyFor(newTowerLevelData))
        {
            return;
        }

        Vector3 position = tower.gameObject.transform.position;
        DestroyImmediate(tower.gameObject);

        var newTower = Instantiate(newTowerLevelData.towerPrefab, position, Quaternion.identity)
            .gameObject.GetComponent<Tower>();

        newTower.Initialize(newTowerLevelData, id);

        towers[id] = newTower;

        eventBusInstance.InvokeEvent(new OnTowerUpgraded { cost = newTowerLevelData.cost });
    }

    public void SellTower(int id)
    {
        if (!towers.TryGetValue(id, out Tower tower))
        {
            return;
        }

        Vector3 position = tower.gameObject.transform.position;

        Destroy(tower.gameObject);
        towers.Remove(id);

        TileGrid.Instance.ChangeBlockOccupancy(position, false);

        eventBusInstance.InvokeEvent(
            new OnTowerSold
            {
                moneyGained = tower.GetMoneyFromSelling()
            });
    }
}