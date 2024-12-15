using Godot;
using System.Collections.Generic;

namespace MyGame.Map
{
    public partial class TileMapLayerGroup : Node
    {
        private readonly List<TileMapLayer> _tileMapLayers = new();
        private Rect2I _rect = new();

        private void GetTileMapLayers()
        {
            foreach (Node node in GetChildren())
            {
                if (node is TileMapLayer tileMapLayer)
                {
                    _tileMapLayers.Add(tileMapLayer);
                    Rect2I usedRect = tileMapLayer.GetUsedRect();
                    Vector2I tileSize = tileMapLayer.TileSet.TileSize;
                    Rect2I rect = new(usedRect.Position * tileSize, usedRect.Size * tileSize);
                    _rect = _rect.Merge(rect);
                }
            }
        }

        public Rect2I GetUsedRect()
        {
            return _rect;
        }

        public override void _Ready()
        {
            GetTileMapLayers();
            GD.Print($"Used rect of map is: {_rect}");
        }
    }
}
