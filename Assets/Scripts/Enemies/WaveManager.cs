using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviourSingleton<WaveManager>
{
    [SerializeField]
    private WaveSO[] waves;

    private int activeEnemies;

    private int currentWaveIndex;

    private Transform startTransform;

    private void Start()
    {
        startTransform = GameObject.FindGameObjectWithTag("StartPosition").transform;
        EventBus.Instance.Subscribe<OnEnemyDestroyed>((Action<IEvent>)OnEnemyRemoved);
        EventBus.Instance.Subscribe<OnEnemyReachedFinish>((Action<IEvent>)OnEnemyRemoved);
    }

    public void StartNextWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            return;
        }

        StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        currentWaveIndex++;
    }

    private IEnumerator SpawnWave(WaveSO wave)
    {
        foreach (EnemyGroup enemyGroup in wave.enemyGroups)
        {
            yield return SpawnEnemyGroup(enemyGroup);
            yield return new WaitForSeconds(wave.groupDelaySeconds);
        }
    }

    private IEnumerator SpawnEnemyGroup(EnemyGroup enemyGroup)
    {
        for (var i = 0; i < enemyGroup.count; i++)
        {
            SpawnEnemy(enemyGroup.enemyPrefab);
            yield return new WaitForSeconds(enemyGroup.spawnDelaySeconds);
        }
    }

    // TODO Implement object pool
    private void SpawnEnemy(GameObject prefab)
    {
        Instantiate(prefab, startTransform.position, Quaternion.identity);
        activeEnemies++;
    }

    private void OnEnemyRemoved(IEvent @event)
    {
        activeEnemies--;
        if (activeEnemies != 0)
        {
            return;
        }

        EventBus.Instance.InvokeEvent(new OnWaveFinished());
    }
}