using UnityEngine;

[CreateAssetMenu(fileName = "TowerLevel", menuName = "Towers/TowerLevelDataSO")]
public class TowerLevelDataSO : ScriptableObject
{
    public Tower towerPrefab;

    public int cost;
    public int cumulativeSellValue;
    public float range;
}