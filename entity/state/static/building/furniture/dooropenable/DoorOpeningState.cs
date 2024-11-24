using Godot;
using MyGame.Util;
using System;

namespace MyGame.Entity.State
{
    public class DoorOpeningState : BasicState<DoorOpenable>
    {
        public override void Enter(DoorOpenable entity)
        {
            GDUtil.ClearAllSignalConnections(entity.AnimationPlayerNode, "animation_finished");
            entity.AnimationPlayerNode.Play("opening");
            entity.AnimationPlayerNode.AnimationFinished += (StringName animationName) => { entity.StateManager.Transit("OpenState", ""); };
            ((Label)entity.Tip).Text = "Opening...";
        }

        public override void Exit(DoorOpenable entity) { }

        public override Tuple<Type, Action> Transit(DoorOpenable entity, string token, params object[] parameters)
        {
            return new Tuple<Type, Action>(typeof(DoorOpenedState), () => { });
        }

        protected override void InitializeStrategies() { }
    }
}
