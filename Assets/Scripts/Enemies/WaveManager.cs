using System;
using System.Collections;
using UnityEngine;

/// <summary>
///     Manages the spawning of enemy waves in the game.
///     Handles wave timing, enemy spawning, and tracking active enemies.
/// </summary>
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

        Debug.Log(waves[currentWaveIndex]);
        StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        currentWaveIndex++;
        EventBus.Instance.InvokeEvent(new OnWaveStarted { index = currentWaveIndex });
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
        EventBus.Instance.InvokeEvent(
            new OnActiveEnemiesNumberChanged { newNumber = activeEnemies });
    }

    private void OnEnemyRemoved(IEvent @event)
    {
        Debug.Log("enemy removed");
        activeEnemies--;
        EventBus.Instance.InvokeEvent(
            new OnActiveEnemiesNumberChanged { newNumber = activeEnemies });

        if (activeEnemies != 0)
        {
            return;
        }

        EventBus.Instance.InvokeEvent(new OnWaveFinished());

        if (currentWaveIndex == waves.Length - 1)
        {
            EventBus.Instance.InvokeEvent(new OnAllWavesFinished());
        }
    }
}