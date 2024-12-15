using Godot;

namespace MyGame.Map
{
    public partial class BasicMap : Node2D
    {
        protected string _name;
        [Export]
        private LandmarkGroup _landmarkGroup;
        [Export]
        private TileMapLayerGroup _tileMapLayerGroup;

        public BasicMap()
        {
            _name = GetType().Name;
        }

        public string GetMapName() { return _name; }

        public Vector2 GetLandmarkPosition(string landmarkName)
        {
            return _landmarkGroup.GetLandmarkPosition(landmarkName);
        }

        public Rect2I GetUsedRect()
        {
            return _tileMapLayerGroup.GetUsedRect();
        }
    }
}
