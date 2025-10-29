using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class EventBus: MonoBehaviour
{
   private readonly Dictionary<Type, object> eventBindings;

   private EventBinding<T> GetEventBinding<T>() where T : IEvent
   {
      var type = typeof(T);

      if (!eventBindings.TryGetValue(type, out object existing))
      {
         var binding = new EventBinding<T>();
         eventBindings[type] = binding;
         return binding;
      }

      return (EventBinding<T>)existing;
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
