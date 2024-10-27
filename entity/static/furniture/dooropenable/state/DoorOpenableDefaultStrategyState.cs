using MyGame.Component;

namespace MyGame.Entity
{
    public class DoorOpenableDefaultStrategyState : IState
    {
        public void OnEnter(IEntity entity)
        {
            ((DoorOpenable)entity).LoadStrategy(() =>
            {
                return new BinaryStateInteractionStrategy((DoorOpenable)entity, "OpenState");
            });
        }
        public void OnExit(IEntity entity) { }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args) { }
    }
}
