using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerUpgradeDatabaseSO", menuName = "Towers/Upgrade Database")]
public class TowerUpgradeDatabaseSO : ScriptableObject
{
    [SerializeField]
    private TowerLevelDataSO[] basicTowers;

    [SerializeField]
    private List<UpgradeEntry> upgrades;

    private Dictionary<TowerLevelDataSO, List<TowerLevelDataSO>> upgradeLookup;

    public void Initialize()
    {
        upgradeLookup = new Dictionary<TowerLevelDataSO, List<TowerLevelDataSO>>();
        foreach (UpgradeEntry entry in upgrades)
        {
            if (!upgradeLookup.ContainsKey(entry.fromLevel))
            {
                upgradeLookup.Add(entry.fromLevel, new List<TowerLevelDataSO>());
            }

            upgradeLookup[entry.fromLevel].Add(entry.toLevel);
        }
    }

    public bool CanUpgrade(TowerLevelDataSO currentLevel)
    {
        return upgradeLookup != null && upgradeLookup.ContainsKey(currentLevel);
    }

    public List<TowerLevelDataSO> GetNextLevels(TowerLevelDataSO currentLevel)
    {
        if (upgradeLookup != null && upgradeLookup.TryGetValue(
            currentLevel,
            out List<TowerLevelDataSO> nextLevels))
        {
            return nextLevels;
        }

        return null;
    }

    public TowerLevelDataSO[] GetBasicTowers()
    {
        return basicTowers;
    }

    [Serializable]
    public class UpgradeEntry
    {
        public TowerLevelDataSO fromLevel;
        public TowerLevelDataSO toLevel;
    }
}