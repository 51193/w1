using MyGame.Manager;

namespace MyGame.Stage
{
	public partial class GamePlay : BaseStage
	{
		private MapTransition _mapTransition;

		private void InitMap()
		{
			_mapTransition.InitMapProcess("Map0");
		}

		public override void _Ready()
		{
			_mapTransition = GetNode<MapTransition>("MapTransition");
			InitMap();
		}
	}
}
