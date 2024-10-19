using Godot;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
    public class EventManager
    {
        private readonly Dictionary<string, Action> _events = new();

        public void RegistrateEvent(string name, Action action)
        {
            if (_events.ContainsKey(name))
            {
                _events[name] += action;
            }
            else
            {
                _events[name] = action;
            }
        }

        public void UnregistrateEvent(string name, Action action = null)
        {
            if (_events.ContainsKey(name))
            {
                if (action != null)
                {
                    _events[name] -= action;
                }
                else
                {
                    _events.Remove(name);
                }
            }
            else
            {
                GD.PrintErr($"Invalid event name in EventManager when unregistrate event: {name}");
            }
        }

        public void TriggerEvent(string name)
        {
            if (_events.TryGetValue(name, out var action))
            {
                action.Invoke();
            }
            else
            {
                GD.PrintErr($"Invalid event name in EventManager when trigger event: {name}");
            }
        }
    }
}
