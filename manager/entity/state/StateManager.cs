using MyGame.Component;
using MyGame.Entity;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
    public class StateManager
    {
        private readonly List<IState> _states;
        private readonly IEntity _entity;

        public StateManager(IEntity entity)
        {
            _entity = entity;
        }

        public void AddState(IState state)
        {
            _states.Add(state);
            state.Enter(_entity);
        }

        public void Update(double delta)
        {
            for (int i = 0; i < _states.Count; i++)
            {
                _states[i].Update(_entity, delta);
                _states[i] = _states[i].HandleStateTransition(_entity);
            }
        }
    }
}
