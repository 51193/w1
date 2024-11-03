using Godot;
using MyGame.Entity;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Component
{
    public class BaseSaveComponent : ISaveComponent
    {
        public Vector2 Position { get; set; }
        public Dictionary<string, IState> States { get; set; }
        public Dictionary<string, Stack<EventIndex>> Events { get; set; }

        public ISaveComponent Next { get; set; }

        public void SaveData(IEntity entity)
        {
            Position = entity.Position;
            States = entity.GetStates();
            Events = entity.GetEvents();
        }

        public void LoadData(IEntity entity)
        {
            entity.Position = Position;
            entity.InitializeStates(States);
            entity.InitializeEvent(Events);
        }
    }
}
