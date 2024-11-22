using MyGame.Entity.Manager;
using MyGame.State;
using MyGame.Strategy;
using System;
namespace MyGame.Entity
{
    public class CharacterDefaultState : BasicState<BasicCharacter>
    {
        public override void Enter(BasicCharacter entity)
        {
        }

        public override void Exit(BasicCharacter entity)
        {
        }

        public override Tuple<Type, Action> Transit(BasicCharacter entity, string token, params object[] parameters)
        {
            return null;
        }

        protected override void InitializeStrategies()
        {
            AddStrategy<FourFaceDirection>(StrategyGroup.ProcessStrategy);
            AddStrategy<FourFaceAnimation>(StrategyGroup.ProcessStrategy);
            AddStrategy<DefaultVelocity>(StrategyGroup.PhysicsProcessStrategy);
            AddStrategy<MoveAndSlidePosition>(StrategyGroup.PhysicsProcessStrategy);
        }
    }
}
