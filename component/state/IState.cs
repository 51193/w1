using Godot;
using MyGame.Entity;

namespace MyGame.Component
{
    public interface IState
    {
        public void Enter(IEntity entity)
        {
            GD.Print($"{entity.EntityName} : {GetType().Name}, enter");
            OnEnter(entity);
        }
        public void OnEnter(IEntity entity);
        public void Exit(IEntity entity)
        {
            GD.Print($"{entity.EntityName} : {GetType().Name}, exit");
            OnExit(entity);
        }
        public void OnExit(IEntity entity);
        public void HandleStateTransition(IEntity entity, string input, params object[] args)
        {
            GD.Print($"{entity.EntityName} : {GetType().Name}, transit");
            OnHandleStateTransition(entity, input, args);
        }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args);

    }
}
