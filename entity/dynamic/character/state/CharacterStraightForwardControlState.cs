using MyGame.Entity.Manager;
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

        public override Tuple<Type, Action> Transit(BasicCharacter entity, string token, params object[] parameters)
        {
            return token switch
            {
                "HardwareInput" => new Tuple<Type, Action>(typeof(CharacterHardwareInputControlState), () => { }),
                _ => null,
            };
        }
        protected override void InitializeStrategies()
        {
            AddStrategy<StraightForwardDirection>(StrategyGroup.PhysicsProcessStrategy);
        }
    }
}
