using MyGame.Manager;

namespace MyGame.Stage
{
	public partial class GamePlay : BasicStage
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
