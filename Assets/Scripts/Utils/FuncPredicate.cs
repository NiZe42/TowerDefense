using System;

/// <summary>
///     Predicate that evaluates based on a function.
/// </summary>
public class FuncPredicate : IPredicate
{
    private readonly Func<bool> predicate;

    public FuncPredicate(Func<bool> predicate)
    {
        this.predicate = predicate;
    }

    public bool Evaluate()
    {
        return predicate();
    }
}