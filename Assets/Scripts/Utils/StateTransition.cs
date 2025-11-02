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