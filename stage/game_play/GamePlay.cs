using Godot;
using MyGame.Manager;

namespace MyGame.Stage
{
	public partial class GamePlay : BasicStage
	{
		[Export]
		private MapTransition _mapTransition;

		private void InitMap()
		{
			_mapTransition.InitMapProcess("Map0");
			//_mapTransition.FromSaveData("test.json");
		}

		public override void _Ready()
		{
			InitMap();
		}
	}
}
