using Godot;
using MyGame.Component;
using MyGame.Strategy;
using MyGame.Util;

namespace MyGame.Entity
{
    public class CharacterHardwareInputControlState : IState
    {
        public void OnEnter(IEntity entity)
        {
            entity.StrategyManager.AddStrategy<InputDirection>(StrategyGroup.PhysicsProcessStrategy);
        }
        public void OnExit(IEntity entity) 
        {
            entity.StrategyManager.RemoveStrategy<InputDirection>(StrategyGroup.PhysicsProcessStrategy);
        }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args)
        {
            switch (input)
            {
                case "GoStraight":
                    if (args.Length > 0 && args[0] is Vector2 position)
                    {
                        entity.RegistrateEvent("OnReachedTarget", typeof(BasicCharacterEvents), "ChangeControlStateToHardwareInputControlState");
                        entity.StateManager.ChangeState("ControlState", new CharacterStraightForwardControlState(position, "OnReachedTarget"));
                    }
                    break;
            }
        }
    }
}
