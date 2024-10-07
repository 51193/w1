using MyGame.Entity;

namespace MyGame.Component
{
    public interface IState
    {
        public void Enter(IEntity entity);
        public void Exit(IEntity entity);
        public void Update(IEntity entity, double delta);
        public IState HandleStateTransition(IEntity entity);

        public string SaveState()
        {
            return GetType().Name;
        }

        public void LoadState(string state);

        public void TransitToNewState(IEntity entity, IState newState)
        {
            Exit(entity);
            newState.Enter(entity);
        }
    }
}
