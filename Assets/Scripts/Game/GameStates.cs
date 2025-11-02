using UnityEngine;

public class BuildingState : IState
{
    public void OnEnterState() { Debug.Log("Building started"); }
    public void Update() { }
    public void OnExitState() { Debug.Log("Building ended"); }
}

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