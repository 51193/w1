using MyGame.Entity.MainBody;
using MyGame.Entity.Manager;
using MyGame.Entity.Strategy;
using MyGame.Strategy;
using System;

namespace MyGame.Entity.State
{
    public class PlayerDefaultState : BasicState<BasicCharacter>
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

            AddStrategy<FindNearestInteractableEntity>(StrategyGroup.PhysicsProcessStrategy);
            AddStrategy<DisplayNearestInteractableEntityTip>(StrategyGroup.ProcessStrategy);
            AddStrategy<PressActivateButtonTrigerInteraction>(StrategyGroup.ProcessStrategy);
        }
    }
}
