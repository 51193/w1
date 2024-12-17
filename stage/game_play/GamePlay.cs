using Godot;
using MyGame.Manager;

namespace MyGame.Stage
{
	public partial class GamePlay : BasicStage
	{
		[Export]
		private MapTransition _mapTransition;

		private SaveConfig _saveConfig;

		public void InitMap(SaveConfig config)
		{
			_saveConfig = config;
			SaveManager.Instance.Load(_saveConfig);
		}

		public override void _Process(double delta)
		{
			if (Input.IsActionJustReleased("save"))
			{
				SaveManager.Instance.Save(_saveConfig);
			}

			if (Input.IsActionJustReleased("load"))
			{
				SaveManager.Instance.Load(_saveConfig);
			}
		}
	}
}
