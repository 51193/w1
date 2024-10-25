using Godot;
using MyGame.Entity;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Component
{
    public class BaseSaveComponent : ISaveComponent
    {
        public Vector2 Position;
        public Dictionary<string, IState> States;
        public Dictionary<string, Stack<EventIndex>> Events;

        private ISaveComponent _next;

        public ISaveComponent Next { get => _next; set => _next = value; }

        public void SaveData(IEntity entity)
        {
            Position = entity.Position;
            States = entity.GetStates();
            Events = entity.GetEvents();
        }

        public void LoadData(IEntity entity)
        {
            entity.Position = Position;
            entity.InitiateStates(States);
            entity.InitiateEvent(Events);
        }
    }
}
