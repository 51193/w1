using Godot;
using MyGame.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyGame.Manager
{
    public class EventIndex
    {
        public string TypeName;
        public string EventName;
        public object[] Parameters;


        public EventIndex(Type type, string eventName, object[] parameters)
        {
            TypeName = type.FullName;
            EventName = eventName;
            Parameters = parameters;
        }

        public Action GetEvent()
        {
            return EventInitiator.GetEvent(TypeName, EventName, Parameters);
        }
    }

    public class EventManager
    {
        private readonly Dictionary<string, Stack<EventIndex>> _events;

        public EventManager(Dictionary<string, Stack<EventIndex>> events)
        {
            if (events == null)
            {
                _events = new();
            }
            else
            {
                _events = events;
            }
        }

        public Dictionary<string, Stack<EventIndex>> GetEvents()
        {
            return _events;
        }

        public void RegistrateEvent(string name, Type type, string actionKey, params object[] parameters)
        {
            if (!_events.ContainsKey(name))
            {
                _events[name] = new();
            }
            _events[name].Push(new EventIndex(type, actionKey, parameters));
            GD.Print($"New event have registrated to EventManager [{name}: {type}-{actionKey}], now {_events[name].Count} method(s) in {name}'s group");
        }

        public void TriggerEvent(string name)
        {
            if (_events.TryGetValue(name, out var events))
            {
                while (events.Count > 0)
                {
                    EventIndex eventIndex = events.Pop();
                    GD.Print($"EventManager invoke {eventIndex.TypeName}-{eventIndex.EventName}");
                    eventIndex.GetEvent().Invoke();
                }
            }
            else
            {
                GD.PrintErr($"Invalid event name in EventManager when trigger event: {name}");
            }
        }
    }
}
