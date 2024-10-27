using Godot;
using MyGame.Entity;
using MyGame.Manager;
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
    }
}
