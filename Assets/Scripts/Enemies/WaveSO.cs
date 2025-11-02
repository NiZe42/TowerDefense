using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSO", menuName = "Waves/WaveSO")]
public class WaveSO : ScriptableObject
{
    public EnemyGroup[] enemyGroups;
    public float groupDelaySeconds;
}

[Serializable]
public class EnemyGroup
{
    public GameObject enemyPrefab;
    public int count;
    public float spawnDelaySeconds = 0.3f;
}