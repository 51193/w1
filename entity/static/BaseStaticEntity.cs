using Godot;

namespace MyGame.Entity
{
	public partial class BaseStaticEntity : StaticBody2D, IEntity
    {
        private string _renderingOrderGroupName;

        protected string _name = "BaseStaticEntity(shouldn't display)";

        public string GetEntityName() { return _name; }

        public string GetRenderingGroupName() { return _renderingOrderGroupName; }

        public void SetRenderingGroupName(string groupName) { _renderingOrderGroupName = groupName; }

        public override void _EnterTree()
        {
            GD.Print($"Static entity enter: {_name}");
        }

        public override void _ExitTree()
        {
            GD.Print($"Static entity exit: {_name}");
        }
    }
}

