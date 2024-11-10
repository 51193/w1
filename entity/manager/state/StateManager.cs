using Godot;
using MyGame.Entity;
using MyGame.State;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.Manager
{
    public class StateManager
    {
        private readonly IEntity _entity;

        private readonly Dictionary<Type, IState> _states = new();

        public List<Type> States
        {
            get
            {
                return _states.Select(s => s.Key).ToList();
            }
            set
            {
                foreach (var t in value)
                {
                    AddState(t);
                }
            }
        }

        public StateManager(IEntity entity) { _entity = entity; }

        public void AddState(Type type)
        {
            if(_states.ContainsKey(type))
            {
                GD.PrintErr($"Add duplicate state: {type.FullName}");
                return;
            }
            _states[type] = StateInstanceManager.Instance.GetInstance(type);
            _states[type].Enter(_entity);
        }

        public void RemoveState(Type type)
        {
            if(!_states.ContainsKey(type))
            {
                GD.PrintErr($"{type.FullName} is not exist when remove state");
                return;
            }
            _states[type].Exit(_entity);
            _states.Remove(type);
        }

        public void Transit(Type type, string token, params object[] parameters)
        {
            if (!_states.TryGetValue(type, out IState state))
            {
                GD.PrintErr($"State '{type.FullName}' is not exist when transit");
                return;
            }
            Type transitType = state.Transit(_entity, token, parameters);
            AddState(transitType);
            RemoveState(type);
        }

        public void Transit<T>(string token, params object[] parameters) where T : IState
        {
            Type type = typeof(T);
            Transit(type, token, parameters);
        }
    }
}
