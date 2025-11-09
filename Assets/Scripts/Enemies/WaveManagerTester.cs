using UnityEngine;

/// <summary>
///     A simple testing helper for the WaveManager.
///     Automatically starts the first wave when the game begins.
/// </summary>
public class WaveManagerTester : MonoBehaviour
{
    private bool isInitialized;
    private WaveManager waveManager;

    private void Start()
    {
        waveManager = GetComponent<WaveManager>();
    }

    private void Update()
    {
        if (!isInitialized)
        {
            waveManager.StartNextWave();
            isInitialized = true;
        }
    }
}