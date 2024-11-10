using System.Collections.Generic;

namespace MyGame.Entity
{
    public class EventContainer
    {
        private readonly Stack<EventIndex> _events = new();

        public void AddEvent(EventIndex index)
        {
            _events.Push(index);
        }

        public void ActivateEvents(IEntity entity)
        {
            while (_events.Count > 0)
            {
                EventIndex index = _events.Pop();
                index.Invoke(entity);
            }
        }
    }
}
