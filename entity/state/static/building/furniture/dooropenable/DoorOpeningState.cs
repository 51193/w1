using Godot;
using MyGame.Entity.Manager;
using MyGame.Entity.Strategy;
using MyGame.Util;
using System;

namespace MyGame.Entity.State
{
    public class DoorOpeningState : BasicState<DoorOpenable>
    {
        public override void Enter(DoorOpenable entity)
        {
            entity.AnimationPlayerNode.Play("opening");
            entity.AnimationPlayerNode.AnimationFinished += (StringName animationName) => { entity.StateManager.Transit("OpenState", ""); };
            ((Label)entity.Tip).Text = "Opening...";
        }

        public override void Exit(DoorOpenable entity)
        {
            GDUtil.ClearAllSignalConnections(entity.AnimationPlayerNode, "animation_finished");
            ((Label)entity.Tip).Text = "";
        }

        public override Tuple<Type, Action> Transit(DoorOpenable entity, string token, params object[] parameters)
        {
            return new Tuple<Type, Action>(typeof(DoorOpenedState), () => { });
        }

        protected override void InitializeStrategies()
        {
            AddStrategy<DoorAnimationTimelineStorage>(StrategyGroup.ProcessStrategy);
        }
    }
}
