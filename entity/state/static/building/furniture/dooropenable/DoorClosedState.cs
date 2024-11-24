using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.Manager;
using MyGame.Entity.Strategy;
using MyGame.Util;
using System;

namespace MyGame.Entity.State
{
    public class DoorClosedState : BasicState<DoorOpenable>
    {
        public override void Enter(DoorOpenable entity)
        {
            GDUtil.ClearAllSignalConnections(entity.AnimationPlayerNode, "animation_finished");

            AccessData<InteractionStrategyTypeData>(entity).InteractionStrategyType = typeof(DoorInteraction);
            ((Label)entity.Tip).Text = "Press E Open";
            entity.AnimationPlayerNode.Play("closed");
        }

        public override void Exit(DoorOpenable entity) { }

        public override Tuple<Type, Action> Transit(DoorOpenable entity, string token, params object[] parameters)
        {
            return new Tuple<Type, Action>(typeof(DoorOpeningState), () => { });
        }

        protected override void InitializeStrategies()
        {
            AddStrategy<DoorInteraction>(StrategyGroup.NormalStrategy);
        }
    }
}
