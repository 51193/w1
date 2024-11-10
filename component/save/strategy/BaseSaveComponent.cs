using Godot;
using MyGame.Entity;
using MyGame.Manager;
using System;
using System.Collections.Generic;

namespace MyGame.Component
{
    public class BaseSaveComponent : ISaveComponent
    {
        public Vector2 Position { get; set; }
        public List<Type> States { get; set; }
        public Dictionary<string, Stack<EventIndex>> Events { get; set; }

        public ISaveComponent Next { get; set; }

        public void SaveData(IEntity entity)
        {
            Position = entity.Position;
            States = entity.StateManager.States;
            Events = entity.GetEvents();
        }

        public void LoadData(IEntity entity)
        {
            entity.Position = Position;
            entity.StateManager.States = States;
            entity.InitializeEvent(Events);
        }
    }
}
