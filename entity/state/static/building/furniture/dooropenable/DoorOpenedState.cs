using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.Manager;
using MyGame.Entity.Strategy;
using System;

namespace MyGame.Entity.State
{
    public class DoorOpenedState : BasicState<DoorOpenable>
    {
        public override void Enter(DoorOpenable entity)
        {
            AccessData<InteractionStrategyTypeData>(entity).InteractionStrategyType = typeof(DoorInteraction);
            ((Label)entity.Tip).Text = "Press E Close";
            entity.AnimationPlayerNode.Play("opened");
        }

        public override void Exit(DoorOpenable entity)
        {
            AccessData<InteractionStrategyTypeData>(entity).InteractionStrategyType = null;
            ((Label)entity.Tip).Text = "";
        }

        public override Tuple<Type, Action> Transit(DoorOpenable entity, string token, params object[] parameters)
        {
            return new Tuple<Type, Action>(typeof(DoorClosingState), () => { });
        }

        protected override void InitializeStrategies()
        {
            AddStrategy<DoorInteraction>(StrategyGroup.NormalStrategy);
        }
    }
}
