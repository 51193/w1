﻿using Godot;

namespace MyGame.Stage
{
    public partial class BaseStage : Node
    {
        protected string _name = "BaseStage(shouldn't display)";

        public override void _EnterTree()
        {
            GD.Print($"Stage enter: {_name}");
        }

        public override void _ExitTree()
        {
            GD.Print($"Stage exit: {_name}");
        }
    }
}