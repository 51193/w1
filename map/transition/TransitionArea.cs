using Godot;
using MyGame.Entity;
using MyGame.Manager;

namespace MyGame.Map
{
	public partial class TransitionArea : Area2D
	{
		[Export]
		string ExitName;

		private BaseMap _map;

		private void OnBodyEntered(Node body)
		{
			if (body is BaseDynamicEntity entity && entity.IsTransitable)
			{
				GlobalObjectManager.EmitTransitMapSignal(_map.GetMapName(), ExitName, Position, entity);
				GD.Print("Transitable entity entered transition area.");
			}
		}

		public override void _ExitTree()
		{
			BodyEntered -= OnBodyEntered;
		}

		public override void _Ready()
		{
			_map = GetParent<BaseMap>();
			BodyEntered += OnBodyEntered;
		}
	}
}
