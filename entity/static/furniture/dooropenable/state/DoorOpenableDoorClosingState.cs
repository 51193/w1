using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
    public class DoorOpenableDoorClosingState : IState
    {
        private DoorOpenable _doorEntity;
        private void OnAnimationFinished(StringName animName)
        {
            _doorEntity.AnimationPlayerNode.AnimationFinished -= OnAnimationFinished;
            _doorEntity.StateManager.ChangeState("OpenState", new DoorOpenableDoorClosedState());
        }
        public void OnEnter(IEntity entity)
        {
            _doorEntity = (DoorOpenable)entity;
            _doorEntity.AnimationPlayerNode.AnimationFinished += OnAnimationFinished;
            _doorEntity.AnimationPlayerNode.Play("closing");
            _doorEntity.Tip.Text = "";
        }
        public void OnExit(IEntity entity) { }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args) { }
    }
}
