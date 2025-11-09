using TMPro;
using UnityEngine;

/// <summary>
///     UI element, that is responsible for showing global game states.
///     Shows Build State with timer and wave number with remaining enemies.
/// </summary>
public class GameStateUI : MonoBehaviour
{
    private int activeEnemiesNumber;
    private float buildTimeRemaining;
    private float buildTimeSeconds;
    private int currentWaveIndex;
    private bool isBuildState = true;

    private bool isInitialized;
    private TextMeshProUGUI stateText;

    public void Awake()
    {
        stateText = GetComponent<TextMeshProUGUI>();
    }

    public void Start()
    {
        EventBus.Instance.Subscribe<OnWaveStarted>(WaveStarted);
        EventBus.Instance.Subscribe<OnActiveEnemiesNumberChanged>(ActiveEnemiesNumberChanged);
        EventBus.Instance.Subscribe<OnBuildStateStarted>(BuildStateStarted);
    }

    private void Update()
    {
        if (!isInitialized)
        {
            buildTimeRemaining = buildTimeSeconds;
            isInitialized      = true;
        }

        if (isBuildState)
        {
            buildTimeRemaining -= Time.deltaTime;
            UpdateBuildUI();
        }
    }

    public void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<OnWaveStarted>(WaveStarted);
            EventBus.Instance.Unsubscribe<OnActiveEnemiesNumberChanged>(ActiveEnemiesNumberChanged);
            EventBus.Instance.Unsubscribe<OnBuildStateStarted>(BuildStateStarted);
        }
    }

    private void WaveStarted(OnWaveStarted @event)
    {
        isBuildState     = false;
        stateText.text   = $"Wave {@event.index}\nEnemies remaining: {activeEnemiesNumber}";
        currentWaveIndex = @event.index;
    }

    private void ActiveEnemiesNumberChanged(OnActiveEnemiesNumberChanged @event)
    {
        activeEnemiesNumber = @event.newNumber;

        if (!isBuildState)
        {
            stateText.text = $"Wave {currentWaveIndex}\nEnemies remaining: {activeEnemiesNumber}";
        }
    }

    private void BuildStateStarted()
    {
        isBuildState       = true;
        buildTimeRemaining = buildTimeSeconds;
        UpdateBuildUI();
    }

    private void UpdateBuildUI()
    {
        stateText.text = $"Building State\nTime left: {buildTimeRemaining:F1}s";
    }

    public void SetBuildTimeSeconds(float seconds)
    {
        buildTimeSeconds = seconds;
    }
}