using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField]
    private float buildingStateDuration;

    private StateMachine stateMachine;

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

    private void Update()
    {
        stateMachine.Update();
    }
}