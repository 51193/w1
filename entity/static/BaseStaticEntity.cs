using Godot;
using MyGame.Component;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public partial class BaseStaticEntity : StaticBody2D, IEntity
    {
        protected StateManager _stateManager;

        private string _renderingOrderGroupName;

        private string _name;

        public BaseStaticEntity()
        {
            _name = GetType().Name;
        }

        public string GetEntityName() { return _name; }

        public string GetRenderingGroupName() { return _renderingOrderGroupName; }

        public void SetRenderingGroupName(string groupName) { _renderingOrderGroupName = groupName; }

        public virtual void InitiateStates(Dictionary<string, IState> states = null)
        {
            if (states == null) 
            {
                _stateManager = new(this);
            }
            else
            {
                _stateManager = new(this, states);
            }
        }

        public Dictionary<string, IState> GetStates()
        {
            return _stateManager.GetStates();
        }

        public void HandleStateTransition(string stateName, string input = null)
        {
            _stateManager.HandleStateTransition(stateName, input);
        }

        public override void _EnterTree()
        {
            GD.Print($"Static entity enter: {_name}");
        }

        public override void _ExitTree()
        {
            GD.Print($"Static entity exit: {_name}");
        }

        public void EntityInitiateProcess() { }
    }
}

