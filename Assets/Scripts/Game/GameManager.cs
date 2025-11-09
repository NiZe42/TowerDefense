using UnityEngine;

/// <summary>
///     Handles the main game flow and state machine.
///     Manages player health, game over, and time scaling.
/// </summary>
public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField]
    private float buildingStateDuration;

    [SerializeField]
    private int totalPlayerHealth;

    private StateMachine stateMachine;

    private float timeMultiplier;

    public override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine();

        var buildState = new BuildingState();
        var waveState  = new WaveState();

        stateMachine.AddPossibleNode(buildState);
        stateMachine.AddPossibleNode(waveState);

        var buildingTimerPredicate     = new TimerPredicate(buildingStateDuration);
        var waveFinishedEventPredicate = new EventPredicate<OnWaveFinished>();

        stateMachine.AddTransition(buildState, waveState, buildingTimerPredicate);
        stateMachine.AddTransition(waveState, buildState, waveFinishedEventPredicate);

        stateMachine.SetCurrentState(buildState);
    }

    public void Start()
    {
        EventBus.Instance.Subscribe<OnEnemyReachedFinish>(ChangePlayerHealth);
        EventBus.Instance.Subscribe<OnAllWavesFinished>(AllWavesFinished);
        UIManager.Instance.GetPlayerHealthUI().SetPlayerHealthText(totalPlayerHealth);
        UIManager.Instance.GetGameStateUI().SetBuildTimeSeconds(buildingStateDuration);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<OnAllWavesFinished>(AllWavesFinished);
            EventBus.Instance.Unsubscribe<OnEnemyReachedFinish>(ChangePlayerHealth);
        }
    }

    private void ChangePlayerHealth(OnEnemyReachedFinish @event)
    {
        totalPlayerHealth -= @event.damage;
        EventBus.Instance.InvokeEvent(
            new OnPlayerHealthChanged { newPlayerHealth = totalPlayerHealth });

        if (totalPlayerHealth <= 0)
        {
            TriggerGameOver(false);
        }
    }

    private void AllWavesFinished()
    {
        TriggerGameOver(true);
    }

    private void TriggerGameOver(bool hasWon)
    {
        Time.timeScale = 0f;
        UIManager.Instance.TriggerEndGame(hasWon);
    }

    public void IncrementTimeMultiplier(bool positively)
    {
        if (positively)
        {
            timeMultiplier += .1f;
            if (timeMultiplier >= 2f)
            {
                timeMultiplier = 2f;
            }
        }
        else
        {
            timeMultiplier -= .1f;
            if (timeMultiplier <= 0f)
            {
                timeMultiplier = 0f;
            }
        }

        Time.timeScale = timeMultiplier;
    }
}