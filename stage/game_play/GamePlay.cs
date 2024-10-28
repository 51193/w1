using MyGame.Manager;
using MyGame.Util;

namespace MyGame.Stage
{
	public partial class GamePlay : BaseStage
	{
		private MapTransition _mapTransition;

		private void InitMap()
		{
			_mapTransition.FromSaveData("test.json");
		}

		public override void _Ready()
		{
			_mapTransition = GetNode<MapTransition>("MapTransition");
			InitMap();
		}
	}
}
