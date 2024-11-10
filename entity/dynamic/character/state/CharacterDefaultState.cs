using Godot;
using MyGame.Component;
using MyGame.State;
using MyGame.Strategy;
using MyGame.Util;
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

        public override Type Transit(BasicCharacter entity, string token, params object[] parameters)
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
