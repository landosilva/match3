using System;
using System.Collections.Generic;

namespace Lando.Plugins.Events
{   
    public class Event
    {
        public delegate void EventDelegate<in T> (T e) where T : IEvent;
        private delegate void EventDelegate(IEvent e);

        private static Event _instance;
        private readonly Dictionary<Type, List<EventDelegate>> _delegates = new();
        private readonly Dictionary<Delegate, EventDelegate> _delegateLookup = new();
        private readonly Stack<EventDelegate> _stack = new();
        
        private static Event Instance => _instance ??= new Event();

        private Event() { }
        
        public static void Subscribe<T> (EventDelegate<T> @delegate) where T : IEvent
        {
            if (@delegate == null || Instance._delegateLookup.ContainsKey(@delegate)) 
                return;

            Instance._delegateLookup[@delegate] = InternalDelegate;

            if (!Instance._delegates.TryGetValue(typeof(T), out List<EventDelegate> list))
            {
                list = new List<EventDelegate>();
                Instance._delegates[typeof(T)] = list;
            }
            list.Add(InternalDelegate);
            
            return;

            void InternalDelegate(IEvent e) => @delegate((T)e);
        }

        public static void Unsubscribe<T> (EventDelegate<T> @delegate) where T : IEvent
        {
            if (@delegate == null) 
                return;
            
            if (!Instance._delegateLookup.TryGetValue(@delegate, out EventDelegate internalDelegate)) 
                return;
            
            if (Instance._delegates.TryGetValue(typeof(T), out List<EventDelegate> list))
            {
                list.Remove(internalDelegate);

                if (list.Count == 0) 
                    Instance._delegates.Remove(typeof(T));
            }

            Instance._delegateLookup.Remove(@delegate);
        }

        public static void Raise(IEvent @event)
        {
            if (!Instance._delegates.TryGetValue(@event.GetType(), out List<EventDelegate> list)) 
                return;
            
            int countBefore = Instance._stack.Count;
            
            for (int i = list.Count-1; i >= 0; i--) 
                Instance._stack.Push(list[i]);

            while (Instance._stack.Count > countBefore)
            {
                EventDelegate eventDelegate = Instance._stack.Pop();
                eventDelegate(@event);
            }
        }
    }
}