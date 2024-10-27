using Godot;
using MyGame.Component;
using MyGame.Util;

namespace MyGame.Entity
{
    public class CharacterHardwareInputControlState : IState
    {
        public void OnEnter(IEntity entity)
        {
            ((DynamicEntity0)entity).LoadStrategy(() =>
            {
                return new InputNavigator();
            });
        }
        public void OnExit(IEntity entity) { }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args)
        {
            switch (input)
            {
                case "GoStraight":
                    if (args.Length > 0 && args[0] is Vector2 position)
                    {
                        entity.RegistrateEvent("OnReachedTarget", typeof(BaseCharacterEvents), "ChangeControlStateToHardwareInputControlState", entity);
                        entity.StateManager.ChangeState("ControlState", new CharacterStraightForwardControlState(position, "OnReachedTarget"));
                    }
                    break;
            }
        }
    }
}
