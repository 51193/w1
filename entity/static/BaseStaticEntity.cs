using Godot;

namespace MyGame.Entity
{
	public partial class BaseStaticEntity : StaticBody2D, IEntity
    {
        private string _renderingOrderGroupName;

        private string _name;

        public BaseStaticEntity()
        {
            _name = GetType().Name;
        }

        public string GetEntityName() { return _name; }

        public string GetRenderingGroupName() { return _renderingOrderGroupName; }

        public void SetRenderingGroupName(string groupName) { _renderingOrderGroupName = groupName; }

        public virtual string GetState()
        {
            return null;
        }

        public virtual void SetState(string state) { }

        public override void _EnterTree()
        {
            GD.Print($"Static entity enter: {_name}");
        }

        public override void _ExitTree()
        {
            GD.Print($"Static entity exit: {_name}");
        }

        public void EntityInitiateProcess() { }
    }
}

