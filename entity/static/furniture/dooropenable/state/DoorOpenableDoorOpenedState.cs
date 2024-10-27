using MyGame.Component;

namespace MyGame.Entity
{
    public class DoorOpenableDoorOpenedState : IState
    {
        public void OnEnter(IEntity entity)
        {
            ((DoorOpenable)entity).AnimationPlayerNode.Play("opened");
            ((DoorOpenable)entity).LabelNode.Text = "按E关门";
        }
        public void OnExit(IEntity entity) { }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args)
        {
            ((DoorOpenable)entity).StateManager.ChangeState("OpenState", new DoorOpenableDoorClosingState());
        }
    }
}
