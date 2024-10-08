using Godot;
using MyGame.Component;
using MyGame.Entity;
using System.Collections.Generic;

namespace MyGame.Manager
{
    public class StateManager
    {
        private readonly Dictionary<string, IState> _states;
        private readonly IEntity _entity;

        public StateManager(IEntity entity, Dictionary<string, IState> states = null)
        {
            _entity = entity;
            if (states != null)
            {
                _states = states;
                foreach (var state in states.Values)
                {
                    state.Enter(_entity);
                }
            }
            else
            {
                _states = new();
            }
        }

        public void HandleStateTransition(string stateName, string input)
        {
            if (_states.ContainsKey(stateName))
            {
                _states[stateName].HandleStateTransition(_entity, input);
            }
            else
            {
                GD.PrintErr($"{stateName} not exist in {_entity.GetEntityName()}, unable to transit state");
            }
        }

        public void ChangeState(string stateName, IState state)
        {
            if (_states.TryGetValue(stateName, out var currentState))
            {
                currentState.Exit(_entity);
            }
            _states[stateName] = state;
            state.Enter(_entity);
        }

        public Dictionary<string, IState> GetStates() { return _states; }
    }
}
