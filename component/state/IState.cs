using MyGame.Entity;

namespace MyGame.Component
{
    public interface IState
    {
        public void Enter(IEntity entity);
        public void Exit(IEntity entity);
        public void HandleStateTransition(IEntity entity, string input);
    }
}
