using System;

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