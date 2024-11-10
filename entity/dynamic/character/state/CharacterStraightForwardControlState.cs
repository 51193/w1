using MyGame.State;
using MyGame.Strategy;
using System;

namespace MyGame.Entity
{
    public class CharacterStraightForwardControlState : BasicState<BasicCharacter>
    {
        public override void Enter(BasicCharacter entity)
        {
            entity.IsTransitable = false;
        }

        public override void Exit(BasicCharacter entity)
        {
            entity.IsTransitable = true;
        }

        public override Type Transit(BasicCharacter entity, string token, params object[] parameters)
        {
            return token switch
            {
                "HardwareInput" => typeof(CharacterHardwareInputControlState),
                _ => null,
            };
        }
        protected override void InitializeStrategies()
        {
            AddStrategy<StraightForwardDirection>(StrategyGroup.PhysicsProcessStrategy);
        }
    }
}
