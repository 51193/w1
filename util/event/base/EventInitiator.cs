using Godot;
using System;
using System.Collections.Generic;

namespace MyGame.Util
{
    public static class EventInitiator
    {
        private static readonly Dictionary<Type, IEventProvider> _eventProviders = new();

        public static Action GetEvent(Type type, string eventName, params object[] parameters)
        {
            if (!_eventProviders.ContainsKey(type))
            {
                IEventProvider eventProvider = (IEventProvider)Activator.CreateInstance(type);
                if (eventProvider == null)
                {
                    GD.PrintErr($"Failed to instantiate {type.FullName} as IEventProvider in EventInitiator");
                    return null;
                }
                _eventProviders[type] = eventProvider;
                GD.Print($"{type} have instantiated as an EventProvider");
            }
            return _eventProviders[type].GetEvent(eventName, parameters);
        }
    }
}
