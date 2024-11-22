using Godot;
using MyGame.Entity;
using MyGame.Entity.Data;
using MyGame.Manager;
using System;
using System.Collections.Generic;

namespace MyGame.Component
{
    public class BaseSaveComponent : ISaveComponent
    {
        public Vector2 Position { get; set; }
        public List<Type> States { get; set; }
        public Dictionary<Type, BasicData> Data { get; set; }

        public ISaveComponent Next { get; set; }

        public void SaveData(IEntity entity)
        {
            Position = entity.Position;
            States = entity.StateManager.StateTypes;
            Data = entity.DataManager.DataDict;
        }

        public void LoadData(IEntity entity)
        {
            entity.Position = Position;
            entity.StateManager.StateTypes = States;
            entity.DataManager.DataDict = Data;
        }
    }
}
