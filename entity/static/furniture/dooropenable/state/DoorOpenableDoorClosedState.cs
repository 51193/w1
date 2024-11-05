using MyGame.Component;

namespace MyGame.Entity
{
    public class DoorOpenableDoorClosedState : IState
    {
        public void OnEnter(IEntity entity)
        {
            ((DoorOpenable)entity).AnimationPlayerNode.Play("closed");
            ((DoorOpenable)entity).Tip.Text = "按E开门";
        }
        public void OnExit(IEntity entity) { }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args)
        {
            ((DoorOpenable)entity).StateManager.ChangeState("OpenState", new DoorOpenableDoorOpeningState());
        }
    }
}
