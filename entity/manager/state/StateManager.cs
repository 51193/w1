using Godot;
using MyGame.Entity.MainBody;
using MyGame.Entity.State;
using MyGame.Manager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.Entity.Manager
{
    public class StateManager
    {
        private readonly IEntity _entity;
        private readonly Dictionary<string, IState> _states = new();

        public Dictionary<string, Type> StateTypes
        {
            get
            {
                return _states.ToDictionary(pair => pair.Key, pair => pair.Value.GetType());
            }
            set
            {
                foreach (var kvp in value)
                {
                    TransitState(kvp.Key, kvp.Value);
                }
            }
        }

        public StateManager(IEntity entity) { _entity = entity; }

        private void TransitState(string name, Type type)
        {
            IState newState = StateInstanceManager.Instance.GetInstance(type);
            newState.LoadStrategies(_entity);
            if (_states.ContainsKey(name))
            {
                _states[name].Exit(_entity);
                _states[name].UnloadStrategies(_entity);
            }
            newState.Enter(_entity);
            _states[name] = newState;
        }

        public void RemoveState(string name)
        {
            if(!_states.ContainsKey(name))
            {
                GD.PrintErr($"State name '{name}' is not exist when removing state");
                return;
            }
            _states[name].Exit(_entity);
            _states.Remove(name);
        }

        public void Transit(string name, string token, params object[] parameters)
        {
            if (!_states.TryGetValue(name, out IState state))
            {
                GD.PrintErr($"State name '{name}' is not exist when transiting");
                return;
            }
            var tuple = state.Transit(_entity, token, parameters);
            Type transitType = tuple.Item1;
            TransitState(name, transitType);
            tuple.Item2.Invoke();
        }
    }
}
