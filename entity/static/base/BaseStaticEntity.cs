using Godot;
using MyGame.Component;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public abstract partial class BaseStaticEntity : StaticBody2D, IEntity
    {
        public StateManager StateManager { get; set; }
        public EventManager EventManager { get; set; }

        public string RenderingOrderGroupName { get; set; }

        public string EntityName { get; init;}

        public BaseStaticEntity()
        {
            EntityName = GetType().Name;
        }

        public virtual void InitiateStates(Dictionary<string, IState> states)
        {
            {
                if (states == null)
                {
                    StateManager = new(this);
                }
                else
                {
                    StateManager = new(this, states);
                }
            }
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

        public void EntityInitiateProcess() { }
    }
}

