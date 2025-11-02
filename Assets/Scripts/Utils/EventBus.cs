using System;
using System.Collections.Generic;
using System.Reflection;

public class EventBus : MonoBehaviourSingleton<EventBus>
{
    private readonly Dictionary<Type, object> eventBindings = new Dictionary<Type, object>();

    public override void Awake()
    {
        base.Awake();
        InitializeForAllEvents();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ClearAllEvents();
    }

    // TODO: revise it
    public void InitializeForAllEvents()
    {
        Type     currentType        = typeof(EventBus);
        Assembly containingAssembly = currentType.Assembly;

        Type[] types = containingAssembly.GetTypes();
        foreach (Type type in types)
        {
            if (!typeof(IEvent).IsAssignableFrom(type) || type.IsInterface || type.IsAbstract)
            {
                continue;
            }

            if (eventBindings.ContainsKey(type))
            {
                continue;
            }

            Type   bindingType     = typeof(EventBinding<>).MakeGenericType(type);
            object bindingInstance = Activator.CreateInstance(bindingType);
            eventBindings[type] = bindingInstance;
        }
    }

    public void ClearAllEvents()
    {
        foreach (object binding in eventBindings.Values)
        {
            MethodInfo clearMethod = binding.GetType().GetMethod("Clear");
            clearMethod?.Invoke(binding, null);
        }

        eventBindings.Clear();
    }

    private EventBinding<T> GetEventBinding<T>() where T : IEvent
    {
        Type type = typeof(T);

        if (!eventBindings.TryGetValue(type, out object existing))
        {
            var binding = new EventBinding<T>();
            eventBindings[type] = binding;
            return binding;
        }

        return(EventBinding<T>)existing;
    }

    public void Subscribe<T>(Action<T> action) where T : IEvent
    {
        EventBinding<T> eventBinding = GetEventBinding<T>();
        eventBinding.AddAction(action);
    }

    public void Unsubscribe<T>(Action<T> action) where T : IEvent
    {
        EventBinding<T> eventBinding = GetEventBinding<T>();
        eventBinding.RemoveAction(action);
    }

    public void Subscribe<T>(Action action) where T : IEvent
    {
        EventBinding<T> eventBinding = GetEventBinding<T>();
        eventBinding.AddAction(action);
    }

    public void Unsubscribe<T>(Action action) where T : IEvent
    {
        EventBinding<T> eventBinding = GetEventBinding<T>();
        eventBinding.RemoveAction(action);
    }

    public void Subscribe<T>(Action<IEvent> action) where T : IEvent
    {
        EventBinding<T> eventBinding = GetEventBinding<T>();
        eventBinding.AddAction(action);
    }

    public void Unsubscribe<T>(Action<IEvent> action) where T : IEvent
    {
        EventBinding<T> eventBinding = GetEventBinding<T>();
        eventBinding.RemoveAction(action);
    }

    public void InvokeEvent<T>(T @event) where T : IEvent
    {
        EventBinding<T> eventBinding = GetEventBinding<T>();
        eventBinding.Invoke(@event);
    }
}