using Godot;
using MyGame.Entity.Manager;
using MyGame.Entity.Save;

namespace MyGame.Entity.MainBody
{
	public abstract partial class BasicDynamicEntity: CharacterBody2D, IEntity
	{
		public StateManager StateManager { get; set; }
		public StrategyManager StrategyManager { get; set; }
		public DataManager DataManager { get; set; }

		public bool IsTransitable = true;

		public string EntityName { get; init; }

        public BasicDynamicEntity()
		{
			EntityName = GetType().Name;
			StrategyManager = new(this);
			StateManager = new(this);
			DataManager = new();
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

		public virtual void AfterInitialize() { }
    }
}
