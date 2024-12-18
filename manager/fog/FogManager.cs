using Godot;

namespace MyGame.Manager
{
    public partial class FogManager : Node
    {
        [Export]
        private Fog _fog;

        private ImageTexture _fogTexture;
        private Image _fogImage;

        private Rect2I _fogRect;

        public override void _Ready()
        {
            Rect2I mapRect = new(new Vector2I(0, 0), new Vector2I(1024, 1024));
            _fog.CreateFogTexture(mapRect.Size);
            _fog.ApplyFogTexture(mapRect.Position);

            Image smallEraseBitmap = _fog.GenerateEraseBitmap(new(20, 20), 20);
            Vector2I smallBitmapPosition = new(0, 0);

            _fog.EraseFog(smallEraseBitmap, smallBitmapPosition);
        }
    }
}
