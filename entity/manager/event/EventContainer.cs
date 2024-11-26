using MyGame.Entity.MainBody;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Manager
{
    public class EventContainer
    {
        public Stack<EventIndex> Events { get; set; } = new();

        public void AddEvent(EventIndex index)
        {
            Events.Push(index);
        }

        public void AddEvent(Type eventProviderType, string eventName, params object[] parameters)
        {
            AddEvent(new EventIndex(eventProviderType, eventName, parameters));
        }

        public void ActivateEvents(IEntity entity)
        {
            while (Events.Count > 0)
            {
                EventIndex index = Events.Pop();
                index.Invoke(entity);
            }
        }
    }
}
