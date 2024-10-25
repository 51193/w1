using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyGame.Manager
{
    public class EventIndex
    {
        public string TypeName;
        public string ActionKey;
        public object[] Parameters;


        public EventIndex(Type type, string actionKey, object[] parameters)
        {
            TypeName = type.FullName;
            ActionKey = actionKey;
            Parameters = parameters;
        }

        public Action GetAction()
        {
            Type type = Type.GetType($"{TypeName}Events");
            MethodInfo eventInfo = type.GetMethod("GetEvent", BindingFlags.Public | BindingFlags.Static);
            if (eventInfo != null)
            {
                return (Action)eventInfo.Invoke(null, new object[] { ActionKey, Parameters });
            }
            else
            {
                GD.PrintErr($"Fail to get event in ActionIndex: {TypeName}-{ActionKey}");
                return null;
            }
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
            if (_events.TryGetValue(name, out var callbacks))
            {
                while (callbacks.Count > 0)
                {
                    EventIndex callback = callbacks.Pop();
                    GD.Print($"EventManager invoke {callback.TypeName}-{callback.ActionKey}");
                    callback.GetAction().Invoke();
                }
            }
            else
            {
                GD.PrintErr($"Invalid event name in EventManager when trigger event: {name}");
            }
        }
    }
}
