using System;

internal interface IEventBinding<T> 
{
    public Action<T> onEvent {get; set;}
    public Action onEventNoArgs {get; set;}
}

internal class EventBinding<T> : IEventBinding<T> where T : IEvent 
{
    public Action<T> onEvent { get; set; }
    public Action onEventNoArgs { get; set; }
    
    public EventBinding(Action<T> onEvent = null, Action onEventNoArgs = null) 
    {
        this.onEventNoArgs = onEventNoArgs ?? delegate { };
        this.onEvent = onEvent ?? delegate { };
    }
    
    public void AddAction(Action<T> action) { onEvent += action; }
    public void RemoveAction(Action<T> action) { onEvent -= action; }
    
    public void AddAction(Action action) { onEventNoArgs += action; }
    public void RemoveAction(Action action) { onEventNoArgs -= action; }

    public void Invoke(T @event) 
    {
        onEvent.Invoke(@event);
        onEventNoArgs.Invoke();
    }

}