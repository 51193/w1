using Godot;
using MyGame.Component;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public abstract partial class BasicDynamicEntity: CharacterBody2D, IEntity
	{
		public StateManager StateManager { get; set; }
		public StrategyManager StrategyManager { get; set; }

		public bool IsTransitable = true;

		public string EntityName { get; init; }

		public Vector2 Direction = Vector2.Zero;
		public Vector2 TargetPosition = Vector2.Zero;
		public EventContainer CallbackOnTargetReached = new();

		public float MaxVelocity = 100;
		public float Acceleration = 2000;
		public float Friction = 1000;

        public BasicDynamicEntity()
		{
			EntityName = GetType().Name;
			StrategyManager = new(this);
			StateManager = new(this);
		}

		public virtual ISaveComponent SaveData(ISaveComponent saveComponent = null)
		{
			BaseSaveComponent save = new();
			save.SaveData(this);
			save.Next = saveComponent;
			return save;
		}

		public virtual ISaveComponent LoadData(ISaveComponent saveComponent)
		{
			saveComponent.LoadData(this);
			return saveComponent.Next;
		}

		public override void _EnterTree()
		{
			GD.Print($"Dynamic entity enter: {EntityName}");
		}

		public override void _ExitTree()
		{
			GD.Print($"Dynamic entity exit: {EntityName}");
		}

        public override void _Process(double delta)
        {
            StrategyManager.Process(delta);
        }

        public override void _PhysicsProcess(double delta)
		{
			StrategyManager.PhysicsProcess(delta);
		}

		public virtual void EntityInitializeProcess() { }

		public virtual void AfterInitialize() { }
    }
}
