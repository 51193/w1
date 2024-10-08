using Godot;
using MyGame.Component;
using System;
using static Godot.WebSocketPeer;

namespace MyGame.Entity
{
    public partial class DoorOpenable : BaseInteractableStaticEntity
    {
        private class DoorOpenedState : IState
        {
            public void Enter(IEntity entity)
            {
                ((DoorOpenable)entity)._animationPlayerNode.Play("opened");
            }
            public void Exit(IEntity entity) { }
            public void HandleStateTransition(IEntity entity, string input, params object[] args)
            {
                ((DoorOpenable)entity)._stateManager.ChangeState("OpenState", new DoorClosingState((DoorOpenable)entity));
            }
        }
        private class DoorClosedState : IState
        {
            public void Enter(IEntity entity)
            {
                ((DoorOpenable)entity)._animationPlayerNode.Play("closed");
            }
            public void Exit(IEntity entity) { }
            public void HandleStateTransition(IEntity entity, string input, params object[] args)
            {
                ((DoorOpenable)entity)._stateManager.ChangeState("OpenState", new DoorOpeningState((DoorOpenable)entity));
            }
        }
        private class DoorOpeningState : IState
        {
            private readonly DoorOpenable _doorEntity;
            public DoorOpeningState(DoorOpenable entity)
            {
                _doorEntity = entity;
                _doorEntity._animationPlayerNode.AnimationFinished += OnAnimationFinished;
            }
            private void OnAnimationFinished(StringName animName)
            {
                _doorEntity._stateManager.ChangeState("OpenState", new DoorOpenedState());
            }
            public void Enter(IEntity entity)
            {
                ((DoorOpenable)entity)._animationPlayerNode.Play("opening");
            }
            public void Exit(IEntity entity) { }
            public void HandleStateTransition(IEntity entity, string input, params object[] args) { }
        }
        private class DoorClosingState : IState
        {
            private readonly DoorOpenable _doorEntity;
            public DoorClosingState(DoorOpenable entity)
            {
                _doorEntity = entity;
                _doorEntity._animationPlayerNode.AnimationFinished += OnAnimationFinished;
            }
            private void OnAnimationFinished(StringName animName)
            {
                _doorEntity._stateManager.ChangeState("OpenState", new DoorClosedState());
            }
            public void Enter(IEntity entity)
            {
                ((DoorOpenable)entity)._animationPlayerNode.Play("closing");
            }
            public void Exit(IEntity entity) { }
            public void HandleStateTransition(IEntity entity, string input, params object[] args) { }
        }
    }
}
