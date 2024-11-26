using MyGame.Entity.MainBody;
using System;

namespace MyGame.Entity.State
{
    public interface IState
    {
        public void UnloadStrategies(IEntity entity);
        public void LoadStrategies(IEntity entity);
        public void Enter(IEntity entity);
        public void Exit(IEntity entity);
        public Tuple<Type, Action> Transit(IEntity entity, string token, params object[] parameters);
    }
}
