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
        public EventManager EventManager { get; set; }
        public string EntityName { get; init; }
        public void InitiateStates(Dictionary<string, IState> states = null);
        public Dictionary<string, IState> GetStates()
        {
            return StateManager.GetStates();
        }
        public void HandleStateTransition(string stateName, string input, params object[] args)
        {
            StateManager.HandleStateTransition(stateName, input, args);
        }
        public ISaveComponent SaveData(ISaveComponent saveComponent = null);
        public ISaveComponent LoadData(ISaveComponent saveComponent);
        public void InitiateEvent(Dictionary<string, Stack<EventIndex>> events = null)
        {
            EventManager = new(this, events);
        }
        public void RegistrateEvent(string eventName, Type type, string actionKey, params object[] parameters)
        {
            EventManager.RegistrateEvent(eventName, type, actionKey, parameters);
        }
        public Dictionary<string, Stack<EventIndex>> GetEvents()
        {
            return EventManager.GetEvents();
        }
        public void EntityInitiateProcess();
    }
}
