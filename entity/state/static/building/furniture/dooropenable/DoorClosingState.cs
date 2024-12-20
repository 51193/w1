﻿using Godot;
using MyGame.Entity.Manager;
using MyGame.Entity.Strategy;
using MyGame.Util;
using System;

namespace MyGame.Entity.State
{
    public class DoorClosingState : BasicState<DoorOpenable>
    {
        public override void Enter(DoorOpenable entity)
        {
            entity.AnimationPlayerNode.Play("closing");
            entity.AnimationPlayerNode.AnimationFinished += (StringName animationName) => { entity.StateManager.Transit("OpenState", ""); };
            ((Label)entity.Tip).Text = "Closing...";
        }

        public override void Exit(DoorOpenable entity)
        {
            GDUtil.ClearAllSignalConnections(entity.AnimationPlayerNode, "animation_finished");
            ((Label)entity.Tip).Text = "";
        }

        public override Tuple<Type, Action> Transit(DoorOpenable entity, string token, params object[] parameters)
        {
            return new Tuple<Type, Action>(typeof(DoorClosedState), () => { });
        }

        protected override void InitializeStrategies()
        {
            AddStrategy<DoorAnimationTimelineStorage>(StrategyGroup.ProcessStrategy);
        }
    }
}
