using System.Collections.Generic;

public class StateNode
{
    public IState state;

    public List<StateTransition> transitions = new List<StateTransition>();

    public StateNode(IState state)
    {
        this.state = state;
    }

    public void AddTransition(StateTransition transition)
    {
        transitions.Add(transition);
    }

    public void OnExitNode()
    {
        foreach (StateTransition transition in transitions)
        {
            transition.Predicate.OnExit();
        }

        state.OnExitState();
    }

    public void OnEnterNode()
    {
        foreach (StateTransition transition in transitions)
        {
            transition.Predicate.OnEnter();
        }

        state.OnEnterState();
    }
}