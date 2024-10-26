using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
    public partial class DoorOpenable : BaseInteractableStaticEntity
    {
        private class DefaultStrategeyState : IState
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
        private class DoorOpenedState : IState
        {
            public void OnEnter(IEntity entity)
            {
                ((DoorOpenable)entity)._animationPlayerNode.Play("opened");
                ((DoorOpenable)entity)._label.Text = "按E关门";
            }
            public void OnExit(IEntity entity) { }
            public void OnHandleStateTransition(IEntity entity, string input, params object[] args)
            {
                ((DoorOpenable)entity).StateManager.ChangeState("OpenState", new DoorClosingState());
            }
        }
        private class DoorClosedState : IState
        {
            public void OnEnter(IEntity entity)
            {
                ((DoorOpenable)entity)._animationPlayerNode.Play("closed");
                ((DoorOpenable)entity)._label.Text = "按E开门";
            }
            public void OnExit(IEntity entity) { }
            public void OnHandleStateTransition(IEntity entity, string input, params object[] args)
            {
                ((DoorOpenable)entity).StateManager.ChangeState("OpenState", new DoorOpeningState());
            }
        }
        private class DoorOpeningState : IState
        {
            private DoorOpenable _doorEntity;
            private void OnAnimationFinished(StringName animName)
            {
                _doorEntity._animationPlayerNode.AnimationFinished -= OnAnimationFinished;
                _doorEntity.StateManager.ChangeState("OpenState", new DoorOpenedState());
            }
            public void OnEnter(IEntity entity)
            {
                _doorEntity = (DoorOpenable)entity;
                _doorEntity._animationPlayerNode.AnimationFinished += OnAnimationFinished;
                _doorEntity._animationPlayerNode.Play("opening");
                _doorEntity._label.Text = "";
            }
            public void OnExit(IEntity entity) { }
            public void OnHandleStateTransition(IEntity entity, string input, params object[] args) { }
        }
        private class DoorClosingState : IState
        {
            private DoorOpenable _doorEntity;
            private void OnAnimationFinished(StringName animName)
            {
                _doorEntity._animationPlayerNode.AnimationFinished -= OnAnimationFinished;
                _doorEntity.StateManager.ChangeState("OpenState", new DoorClosedState());
            }
            public void OnEnter(IEntity entity)
            {
                _doorEntity = (DoorOpenable)entity;
                _doorEntity._animationPlayerNode.AnimationFinished += OnAnimationFinished;
                _doorEntity._animationPlayerNode.Play("closing");
                _doorEntity._label.Text = "";
            }
            public void OnExit(IEntity entity) { }
            public void OnHandleStateTransition(IEntity entity, string input, params object[] args) { }
        }
    }
}
