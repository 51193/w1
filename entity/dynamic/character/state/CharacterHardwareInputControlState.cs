using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.Manager;
using MyGame.State;
using MyGame.Strategy;
using MyGame.Util;
using System;

namespace MyGame.Entity
{
    public class CharacterHardwareInputControlState : BasicState<BasicCharacter>
    {
        public override void Enter(BasicCharacter entity)
        {
        }

        public override void Exit(BasicCharacter entity)
        {
        }

        public override Tuple<Type, Action> Transit(BasicCharacter entity, string token, params object[] parameters)
        {
            switch (token)
            {
                case "GoStraight":
                    if (parameters.Length > 0 && parameters[0] is Vector2 position)
                    {
                        return new Tuple<Type, Action>(typeof(CharacterStraightForwardControlState),
                            () =>
                            {
                                entity.DataManager.Get<GoStraightData>().TargetPosition = position;
                                entity.DataManager.Get<GoStraightData>().CallbackOnTargetReached.AddEvent(typeof(BasicCharacterEvents), "ChangeControlStateToHardwareInputControlState");
                            });
                    }
                    return null;
                default:
                    return null;
            }
        }

        protected override void InitializeStrategies()
        {
            AddStrategy<InputDirection>(StrategyGroup.PhysicsProcessStrategy);
        }
    }
}
