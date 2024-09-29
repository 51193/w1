using Godot;
using MyGame.Entity;
using MyGame.Manager;

namespace MyGame.Map
{
	public partial class TransitionArea : Area2D
	{
		[Export]
		string exitName;

		BaseMap map;

		private void OnBodyEntered(Node body)
		{
			if (body is BaseEntity entity && entity.isTransitable && !entity.isLocked)
			{
				GlobalObjectManager.EmitTransitMapSignal(map.GetMapName(), exitName, Position, entity);
				GD.Print("Transitable entity entered transition area.");
			}
		}

		public override void _ExitTree()
		{
			BodyEntered -= OnBodyEntered;
		}

		public override void _Ready()
		{
			map = GetParent<BaseMap>();
			BodyEntered += OnBodyEntered;
		}

		public override void _Process(double delta)
		{
		}
	}
}
