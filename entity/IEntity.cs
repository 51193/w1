using Godot;
using MyGame.Component;
using System;
using System.Collections.Generic;

namespace MyGame.Entity
{
    public interface IEntity
    {
        public Vector2 Position { get; set; }
        public string GetRenderingGroupName();
        public void SetRenderingGroupName(string groupName);
        public string GetEntityName();
        public void InitiateStates(Dictionary<string, IState> states = null);
        public Dictionary<string, IState> GetStates();
        public ISaveComponent SaveData(ISaveComponent saveComponent = null);
        public ISaveComponent LoadData(ISaveComponent saveComponent);
        public void HandleStateTransition(string stateName, string input, params object[] args);
        public void RegistrateEvent(string eventName, Action action);
        public void EntityInitiateProcess();
    }
}
