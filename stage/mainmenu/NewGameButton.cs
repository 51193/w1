using Godot;
using MyGame.Manager;

namespace MyGame.Stage {
	public partial class NewGameButton : Button
	{
		public override void _Ready()
		{
			ButtonUp += () => GlobalObjectManager.EnterStage("GamePlayStage");
		}

		public override void _Process(double delta)
		{
		}
	}
}
