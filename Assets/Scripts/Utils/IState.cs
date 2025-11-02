public interface IState
{
    public void OnEnterState();
    public void Update();
    public void OnExitState();
}