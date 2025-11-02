using System;
using System.Collections.Generic;
using System.Linq;

public class StateMachine
{
    private readonly Dictionary<Type, StateNode> nodes = new Dictionary<Type, StateNode>();
    private StateNode currentNode;

    // Overwrites any nodes that were here of this type
    public StateNode AddPossibleNode(IState state)
    {
        if (state == null)
        {
            return null;
        }

        nodes[state.GetType()] = new StateNode(state);
        return nodes[state.GetType()];
    }

    public void Update()
    {
        if (currentNode == null)
        {
            return;
        }

        foreach (StateTransition transition in currentNode.transitions.Where(transition =>
            transition.Predicate.Evaluate()))
        {
            ChangeState(transition.NextState);
            break;
        }

        currentNode?.state.Update();
    }

    public void SetCurrentState(IState nextState)
    {
        if (!nodes.ContainsKey(nextState.GetType()))
        {
            return;
        }

        currentNode = nodes[nextState.GetType()];
        currentNode.OnEnterNode();
    }

    // if abruptly, onExit from current state is not called.
    private void ChangeState(IState nextState)
    {
        if (nextState == currentNode.state || nextState == null)
        {
            return;
        }

        Type nextType = nextState.GetType();
        if (!nodes.ContainsKey(nextType))
        {
            return;
        }

        currentNode?.OnExitNode();

        currentNode = nodes[nextType];

        currentNode.OnEnterNode();
    }

    public StateNode GetPossibleStateNode(IState state)
    {
        return state == null ? null : nodes[state.GetType()];
    }

    public void AddTransition(IState pastState, IState nextState, IPredicate predicate)
    {
        if (pastState == null || nextState == null)
        {
            return;
        }

        StateNode pastNode = GetPossibleStateNode(pastState);
        if (pastNode != null)
        {
            pastNode.AddTransition(new StateTransition(nextState, predicate));
        }
        else
        {
            AddPossibleNode(pastState).AddTransition(new StateTransition(nextState, predicate));
        }
    }
}