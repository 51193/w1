using Godot;
using System;
using System.Collections.Generic;

namespace MyGame.Util
{
    public static class EventInitiator
    {
        private static readonly Dictionary<string, IEventProvider> _eventProviders = new();
        
        public static Action GetEvent(string typeFullName, string eventName, params object[] parameters)
        {
            if(!_eventProviders.ContainsKey(typeFullName))
            {
                Type providerType = Type.GetType(typeFullName);
                IEventProvider eventProvider = (IEventProvider)Activator.CreateInstance(providerType);
                if (eventProvider == null)
                {
                    GD.PrintErr($"Failed to instantiate {typeFullName} as IEventProvider in EventInitiator");
                    return null;
                }
                _eventProviders[typeFullName] = eventProvider;
                GD.Print($"{typeFullName} have instantiated as an EventProvider");
            }
            return _eventProviders[typeFullName].GetEvent(eventName, parameters);
        }
    }
}
