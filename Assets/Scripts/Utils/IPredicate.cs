/// <summary>
///     Base contract for predicates.
/// </summary>
public interface IPredicate
{
    public bool Evaluate();
    public void OnEnter() { }
    public void OnExit() { }
}