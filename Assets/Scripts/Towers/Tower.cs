using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private GameObject towerVisualsPrefab;

    [SerializeField]
    private ShootingBehaviour shootingBehaviour;

    private TowerLevelDataSO towerData;

    private TowerVisuals towerVisualsInstance;
    public int id { get; private set; }

    public void Awake()
    {
        towerVisualsInstance = Instantiate(
            towerVisualsPrefab,
            transform.position,
            Quaternion.identity,
            transform).GetComponent<TowerVisuals>();
    }

    public void Initialize(TowerLevelDataSO towerData, int id)
    {
        this.towerData = towerData;
        this.id        = id;

        shootingBehaviour.Initialize(
            towerData.damage,
            transform,
            towerData.range,
            towerData.attackCooldown);
    }

    public int GetMoneyFromSelling()
    {
        return towerData.cumulativeSellValue;
    }

    public List<TowerLevelDataSO> GetUpgradeOptions()
    {
        return TowerManager.Instance.towerUpgradeDatabase.GetNextLevels(towerData);
    }

    public TowerLevelDataSO GetTowerData()
    {
        return towerData;
    }
}