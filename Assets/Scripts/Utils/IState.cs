/// <summary>
///     Base contract for a state for <see cref="StateMachine" />>
/// </summary>
public interface IState
{
    public void OnEnterState();
    public void Update();
    public void OnExitState();
}