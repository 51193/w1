using Godot;
using System.Collections.Generic;

namespace MyGame.Map
{
    public partial class BasicMap : Node2D
    {
        protected string _name;
        [Export]
        private LandmarkGroup _landmarkGroup;
        [Export]
        private TileMapLayerGroup _tilemapGroup;

        public BasicMap()
        {
            _name = GetType().Name;
        }

        public string GetMapName() {  return _name; }

        public Vector2 GetLandmarkPosition(string landmarkName)
        {
            return _landmarkGroup.GetLandmarkPosition(landmarkName);
        }

        public Rect2I GetUsedRect()
        {
            return _tilemapGroup.GetUsedRect();
        }
    }
}
