using MyGame.Entity;
using MyGame.Manager;

namespace MyGame.Stage
{
	public partial class GamePlay : BaseStage
	{
		private MapTransition _mapTransition;

		public GamePlay()
		{
			_name = "GamePlayStage";
		}

		private void InitMap(string mapName)
		{
			_mapTransition.EmitSignal(nameof(_mapTransition.InitMap), mapName);
		}

		public override void _Ready()
		{
			_mapTransition = GetNode<MapTransition>("MapTransition");
			InitMap("Map0");
		}

		public override void _Process(double delta)
		{

		}
	}
}
