/// <summary>
///     Initializes core game systems and sets up references between managers at runtime.
/// </summary>
public class Bootstrapper : MonoBehaviourSingleton<Bootstrapper>
{
    private void Start()
    {
        PathFindingManager pathFindingManager = PathFindingManager.Instance;
        TileGrid           tileGrid           = TileGrid.Instance;
        tileGrid.pathValidator = pathFindingManager;

        PlayerEconomyManager playerEconomyManager = PlayerEconomyManager.Instance;
        TowerManager         towerManager         = TowerManager.Instance;
        towerManager.economyValidator = playerEconomyManager;

        UIManager uiManager = UIManager.Instance;
        uiManager.economyValidator = playerEconomyManager;
    }
}