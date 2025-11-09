/// <summary>
///     Transition that defines next state and a predicate that need to evaluate to true for this transition to happen.
/// </summary>
public class StateTransition
{
    public StateTransition(IState nextState, IPredicate predicate)
    {
        NextState = nextState;
        Predicate = predicate;
    }

    public IState NextState { get; private set; }
    public IPredicate Predicate { get; private set; }
}