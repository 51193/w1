using MyGame.Manager;

namespace MyGame.Stage
{
	public partial class GamePlay : BaseStage
	{
		private MapTransition _mapTransition;

		private void InitMap(string mapName)
		{
			_mapTransition.InitMapProcess(mapName);
		}

		public override void _Ready()
		{
			_mapTransition = GetNode<MapTransition>("MapTransition");
			InitMap("Map0");
		}
	}
}
