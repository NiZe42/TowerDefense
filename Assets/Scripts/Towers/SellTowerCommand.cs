/// <summary>
///     Part of command pattern.
///     Executes selling of a tower.
/// </summary>
public class SellTowerCommand : ICommand
{
    private readonly int towerId;

    public SellTowerCommand(int towerId)
    {
        this.towerId = towerId;
    }

    private TowerManager towerManagerInstance => TowerManager.Instance;

    public void Execute()
    {
        towerManagerInstance.SellTower(towerId);
    }
}