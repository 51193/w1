using Godot;
using MyGame.Component;
using MyGame.Entity.Manager;

namespace MyGame.Entity
{
	public abstract partial class BasicStaticEntity : StaticBody2D, IEntity
    {
        public StateManager StateManager { get; set; }
        public StrategyManager StrategyManager { get; set; }
        public DataManager DataManager { get; set; }

        public string EntityName { get; init;}

        public BasicStaticEntity()
        {
            EntityName = GetType().Name;
            StateManager = new(this);
            StrategyManager = new(this);
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
            GD.Print($"Static entity enter: {EntityName}");
        }

        public override void _ExitTree()
        {
            GD.Print($"Static entity exit: {EntityName}");
        }

        public virtual void AfterInitialize() { }
    }
}

