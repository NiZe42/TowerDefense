using UnityEngine;

/// <summary>
///     One of 2 main states of <see cref="GameManager" />.
///     Building phase: players can place towers.
/// </summary>
public class BuildingState : IState
{
    public void OnEnterState()
    {
        Debug.Log("Building started");
        EventBus.Instance.InvokeEvent(new OnBuildStateStarted());
    }

    public void Update() { }
    public void OnExitState() { Debug.Log("Building ended"); }
}

/// <summary>
///     One of 2 main states of <see cref="GameManager" />.
///     Wave state: Enemies are spawning.
/// </summary>
public class WaveState : IState
{
    public void OnEnterState()
    {
        WaveManager.Instance.StartNextWave();
        Debug.Log("Enemies spawned");
    }

    public void Update() { }
    public void OnExitState() { Debug.Log("Enemies defeated"); }
}