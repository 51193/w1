using Godot;
using MyGame.Entity;

namespace MyGame.Component
{
    public class OpenDoorStrategy : IInteractionStrategy
    {
        private enum DOOR_STATE
        {
            CLOSING,
            CLOSED,
            OPENING,
            OPENED
        }

        private readonly AnimationPlayer _animationPlayer;
        private DOOR_STATE _state = DOOR_STATE.CLOSED;

        public OpenDoorStrategy(AnimationPlayer animationPlayer)
        {
            _animationPlayer = animationPlayer;
            _animationPlayer.AnimationFinished += OnAnimationFinished;
        }

        private void OnAnimationFinished(StringName animName)
        {
            switch (animName)
            {
                case "closing":
                    _state = DOOR_STATE.CLOSED;
                    _animationPlayer.Play("closed");
                    break;
                case "opening":
                    _state = DOOR_STATE.OPENED;
                    _animationPlayer.Play("opened");
                    break;
            }
        }

        public void Interaction(BaseInteractableDynamicEntity dynamicEntity)
        {
            switch (_state)
            {
                case DOOR_STATE.CLOSED:
                    _animationPlayer.Play("opening");
                    _state = DOOR_STATE.OPENING;
                    break;
                case DOOR_STATE.OPENED:
                    _animationPlayer.Play("closing");
                    _state = DOOR_STATE.CLOSING;
                    break;
                default:
                    break;
            }
        }

        public string GetState()
        {
            return _state switch
            {
                DOOR_STATE.CLOSING or DOOR_STATE.CLOSED => "closed",
                DOOR_STATE.OPENING or DOOR_STATE.OPENED => "opened",
                _ => null,
            };
        }

        public void SetState(string state)
        {
            switch (state)
            {
                default:
                case "closed":
                    _state = DOOR_STATE.CLOSED;
                    _animationPlayer.Play("closed");
                    break;
                case "opened":
                    _state= DOOR_STATE.OPENED;
                    _animationPlayer.Play("opened");
                    break;
            }
        }
    }
}
