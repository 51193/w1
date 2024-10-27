using Godot;
using MyGame.Entity;
using MyGame.Manager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.Component
{
    public class BaseSaveComponent : ISaveComponent
    {
        public Vector2 Position { get; set; }
        private Dictionary<string, IState> _states;
        public Dictionary<string, StateData> States
        {
            get
            {
                if (_states == null) return new();
                return _states.ToDictionary
                (
                kvp => kvp.Key,
                kvp => kvp.Value.ToStateData()
                );
            }
            set
            {
                _states = new();
                if (value == null)
                {
                    return;
                }
                foreach (var kvp in value)
                {
                    IState state = FromStateData(kvp.Value);
                    if (state != null)
                    {
                        _states[kvp.Key] = state;
                    }
                }
            }
        }
        public Dictionary<string, Stack<EventIndex>> Events { get; set; }

        public ISaveComponent Next { get; set; }

        public void SaveData(IEntity entity)
        {
            Position = entity.Position;
            _states = entity.GetStates();
            Events = entity.GetEvents();
        }

        public void LoadData(IEntity entity)
        {
            entity.Position = Position;
            entity.InitiateStates(_states);
            entity.InitiateEvent(Events);
        }

        private static IState FromStateData(StateData stateData)
        {
            Type type = Type.GetType(stateData.StateTypeName);
            if (type == null)
            {
                GD.PrintErr($"Type '{stateData.StateTypeName}' not found");
                return null;
            }

            IState instance = (IState)Activator.CreateInstance(type);

            foreach (var kvp in stateData.Properties)
            {
                var property = type.GetProperty(kvp.Key);
                if (property == null)
                {
                    GD.PrintErr($"Property '{kvp.Key}' not found in {type.FullName}");
                    return null;
                }
                property.SetValue(instance, kvp.Value);
            }
            return instance;
        }
    }
}
