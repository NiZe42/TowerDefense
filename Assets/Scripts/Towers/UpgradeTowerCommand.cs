/// <summary>
///     Part of a command pattern.
///     Executes an Upgrade of a tower.
/// </summary>
public class UpgradeTowerCommand : ICommand
{
    private readonly TowerLevelDataSO newTowerLevelData;
    private readonly int towerId;

    public UpgradeTowerCommand(int towerId, TowerLevelDataSO newTowerLevelData)
    {
        this.towerId           = towerId;
        this.newTowerLevelData = newTowerLevelData;
    }

    private TowerManager towerManagerInstance => TowerManager.Instance;

    public void Execute()
    {
        towerManagerInstance.UpgradeTower(towerId, newTowerLevelData);
    }
}