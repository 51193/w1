using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
    public class DoorOpenableDoorOpeningState : IState
    {
        private DoorOpenable _doorEntity;
        private void OnAnimationFinished(StringName animName)
        {
            _doorEntity.AnimationPlayerNode.AnimationFinished -= OnAnimationFinished;
            _doorEntity.StateManager.ChangeState("OpenState", new DoorOpenableDoorOpenedState());
        }
        public void OnEnter(IEntity entity)
        {
            _doorEntity = (DoorOpenable)entity;
            _doorEntity.AnimationPlayerNode.AnimationFinished += OnAnimationFinished;
            _doorEntity.AnimationPlayerNode.Play("opening");
            _doorEntity.LabelNode.Text = "";
        }
        public void OnExit(IEntity entity) { }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args) { }
    }
}
