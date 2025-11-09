using UnityEngine;

/// <summary>
///     A test helper class, that automatically initializes tower by tower data.
/// </summary>
public class TowerTestInitializer : MonoBehaviour
{
    [SerializeField]
    private TowerLevelDataSO towerLevelData;

    private Tower tower;

    private void Awake()
    {
        tower = GetComponent<Tower>();
    }

    private void Start()
    {
        tower.Initialize(towerLevelData, 0);
    }
}