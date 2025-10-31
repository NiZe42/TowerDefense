using System;

internal interface IEventBinding<T> 
{
    public Action<T> onEvent {get; set;}
    public Action onEventNoArgs {get; set;}
}

internal class EventBinding<T> : IEventBinding<T> where T : IEvent 
{
    // 1 function for 1 event.
    public Action<T> onEvent { get; set; }
    
    // 1 parameterless function for any events.
    public Action onEventNoArgs { get; set; }
    
    // 1 parametered function for any/many events.
    public Action<IEvent> onEventUntyped { get; set; }
    
    public EventBinding() : this(null, null, null) {}
    
    public EventBinding(Action<T> onEvent = null, Action onEventNoArgs = null,  Action<IEvent> onEventUntyped = null) 
    {
        this.onEventNoArgs = onEventNoArgs ?? delegate { };
        this.onEvent = onEvent ?? delegate { };
        this.onEventUntyped = onEventUntyped ?? delegate { };
    }
    
    public void AddAction(Action<T> action) { onEvent += action; }
    public void RemoveAction(Action<T> action) { onEvent -= action; }
    
    public void AddAction(Action action) { onEventNoArgs += action; }
    public void RemoveAction(Action action) { onEventNoArgs -= action; }
    
    public void AddAction(Action<IEvent> action) { onEventUntyped += action; }
    public void RemoveAction(Action<IEvent> action) { onEventUntyped -= action; }

    public void Invoke(T @event) 
    {
        onEvent.Invoke(@event);
        onEventNoArgs.Invoke();
        onEventUntyped.Invoke(@event);
    }

}