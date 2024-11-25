using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.MainBody;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Save
{
    public class BaseSaveComponent : ISaveComponent
    {
        public Vector2 Position { get; set; }
        public Dictionary<string, Type> States { get; set; }
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
