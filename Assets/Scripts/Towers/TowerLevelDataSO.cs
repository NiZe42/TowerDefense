using UnityEngine;

[CreateAssetMenu(fileName = "TowerLevel", menuName = "Towers/TowerLevelDataSO")]
public class TowerLevelDataSO : ScriptableObject
{
    public string towerName;
    public Tower towerPrefab;
    public Sprite towerIcon;

    public int cost;
    public int cumulativeSellValue;
    public float range;
    public int damage;
    public int attackCooldown;
    public ShootingType shootingType;
}