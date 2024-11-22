using MyGame.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.Entity.Manager
{
    public class VariableLengthParameter
    {
        public List<object> Parameters { get; set; }
        public VariableLengthParameter(params object[] parameters)
        {
            Parameters = new();
            foreach (object parameter in parameters)
            {
                Parameters.Add(parameter);
            }
        }
        public VariableLengthParameter(List<object> parameters)
        {
            Parameters = parameters;
        }
    }

    public class EventIndex
    {
        public Type EventProviderType { get; set; }
        public string EventName { get; set; }
        public VariableLengthParameter Parameters { get; set; }

        public EventIndex(Type type, string eventName, params object[] parameters)
        {
            EventProviderType = type;
            EventName = eventName;
            Parameters = new(parameters);
        }

        public Action GetEvent(IEntity entity)
        {
            return EventInitializer.GetEvent(EventProviderType, EventName, new object[] { entity }.Concat(Parameters.Parameters.ToArray()).ToArray());
        }

        public void Invoke(IEntity entity)
        {
            GetEvent(entity).Invoke();
        }
    }
}
