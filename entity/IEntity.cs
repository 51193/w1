using Godot;
using MyGame.Component;
using MyGame.Manager;
using System;
using System.Collections.Generic;

namespace MyGame.Entity
{
    public interface IEntity
    {
        public Node2D GetNode()
        {
            if (this is Node2D node) return node;
            else return null;
        }
        public Vector2 Position { get; set; }
        public StateManager StateManager { get; set; }
        public StrategyManager StrategyManager { get; set; }
        public string EntityName { get; init; }
        public ISaveComponent SaveData(ISaveComponent saveComponent = null);
        public ISaveComponent LoadData(ISaveComponent saveComponent);
        public void EntityInitializeProcess();
        public void AfterInitialize();
    }
}
