using MyGame.Entity;
using System;

namespace MyGame.State
{
    public interface IState
    {
        public void Enter(IEntity entity);
        public void Exit(IEntity entity);
        public Type Transit(IEntity entity, string token, params object[] parameters);
    }
}
