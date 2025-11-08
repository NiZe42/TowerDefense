// BuildTowerCommand.cs

using UnityEngine;

public class BuildTowerCommand : ICommand
{
    private readonly Vector3 position;
    private readonly TowerLevelDataSO towerLevelData;

    public BuildTowerCommand(Vector3 position, TowerLevelDataSO towerLevelData)
    {
        this.position       = position;
        this.towerLevelData = towerLevelData;
    }

    private TowerManager towerManagerInstance => TowerManager.Instance;

    public void Execute()
    {
        towerManagerInstance.BuildTower(position, towerLevelData);
    }
}