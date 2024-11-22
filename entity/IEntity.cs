using Godot;
using MyGame.Component;
using MyGame.Entity.Manager;

namespace MyGame.Entity
{
    public interface IEntity
    {
        public Node2D GetNode()
        {
            if (this is Node2D node) return node;
            else
            {
                GD.PrintErr($"{EntityName} is not a node, fail to get node");
                return null;
            }
        }
        public Vector2 Position { get; set; }
        public StateManager StateManager { get; set; }
        public StrategyManager StrategyManager { get; set; }
        public DataManager DataManager { get; set; }
        public string EntityName { get; init; }
        public ISaveComponent SaveData(ISaveComponent saveComponent = null);
        public ISaveComponent LoadData(ISaveComponent saveComponent);
        public void AfterInitialize();
    }
}
