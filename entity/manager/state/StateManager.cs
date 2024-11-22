using Godot;
using MyGame.Manager;
using MyGame.State;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.Entity.Manager
{
    public class StateManager
    {
        private readonly IEntity _entity;

        private readonly Dictionary<Type, IState> _states = new();

        public List<Type> StateTypes
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
            var tuple = state.Transit(_entity, token, parameters);
            Type transitType = tuple.Item1;
            AddState(transitType);
            RemoveState(type);
            tuple.Item2.Invoke();
        }

        public void Transit<T>(string token, params object[] parameters) where T : IState
        {
            Type type = typeof(T);
            Transit(type, token, parameters);
        }
    }
}
