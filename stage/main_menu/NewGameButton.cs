using Godot;
using MyGame.Manager;

namespace MyGame.Stage {
    public partial class NewGameButton : Button
    {
        public override void _Pressed()
        {
            GlobalObjectManager.EnterStage("GamePlay");
        }
    }
}
