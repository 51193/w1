using Godot;
using MyGame.Entity;

namespace MyGame.Manager
{
	public partial class UnifiedEntityManager : Node
	{
		private DynamicEntityManager _dynamicEntityManager;
		private StaticEntityManager _staticEntityManager;

		private string _currentMapName;

		private bool _staticEntityManagerReady = false;
		private bool _dynamicEntityManagerReady = false;

		[Signal]
		public delegate void EntityTransitionCompleteEventHandler();

		public void InitiateEntities(string mapName)
		{
			_staticEntityManager.InitiateEntities(mapName);
			_dynamicEntityManager.InitiateEntities(mapName);
			_currentMapName = mapName;
		}

		public void OnMapChanged(BaseDynamicEntity entity, string mapName, Vector2 fromPosition, Vector2 toPosition)
		{
			GlobalObjectManager.EmitClearNodeFromRenderingOrderGroupSignal(_currentMapName);
			_staticEntityManager.OnMapChanged(_currentMapName, mapName);
			_dynamicEntityManager.OnMapChanged(entity, _currentMapName, mapName, fromPosition, toPosition);
			_currentMapName = mapName;
		}

		private void OnStaticEntityManagerComplete()
		{
			_staticEntityManagerReady = true;
			CheckEntityManagerState();
		}

		private void OnDynamicEntityManagerComplete()
		{
			_dynamicEntityManagerReady = true;
			CheckEntityManagerState();
		}

		private void CheckEntityManagerState()
		{
			if (!_staticEntityManagerReady || !_dynamicEntityManagerReady) return;

			EmitSignal(SignalName.EntityTransitionComplete);

			_staticEntityManagerReady = false;
			_dynamicEntityManagerReady = false;
		}

		public void AfterTransitionComplete()
		{
			_staticEntityManager.AfterTransitionComplete();
			_dynamicEntityManager.AfterTransitionComplete();
		}

		public override void _ExitTree()
		{
			_dynamicEntityManager.EntityTransitionComplete -= OnDynamicEntityManagerComplete;
			_staticEntityManager.EntityTransitionComplete -= OnStaticEntityManagerComplete;
		}

		public override void _Ready()
		{
			_dynamicEntityManager = GetNode<DynamicEntityManager>("DynamicEntityManager");
			_staticEntityManager = GetNode<StaticEntityManager>("StaticEntityManager");

			_dynamicEntityManager.EntityTransitionComplete += OnDynamicEntityManagerComplete;
			_staticEntityManager.EntityTransitionComplete += OnStaticEntityManagerComplete;
		}
	}
}
