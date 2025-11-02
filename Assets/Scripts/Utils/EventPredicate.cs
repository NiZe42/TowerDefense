public class EventPredicate<TEvent> : IPredicate where TEvent : IEvent
{
    private bool isTriggered;

    public bool Evaluate()
    {
        return isTriggered;
    }

    public void OnEnter()
    {
        isTriggered = false;
        EventBus.Instance.Subscribe<TEvent>(OnEventFired);
    }

    public void OnExit()
    {
        EventBus.Instance.Unsubscribe<TEvent>(OnEventFired);
    }

    private void OnEventFired()
    {
        isTriggered = true;
    }
}