using Godot;

namespace MyGame.Manager
{
    public partial class Fog : Node2D
    {
        [Export]
        private Sprite2D _fogSprite;

        private ImageTexture _fogTexture;
        private Image _fogImage;

        private Rect2I _fogRect;

        public void CreateFogImage(Vector2I size)
        {
            _fogImage = Image.CreateEmpty(size.X, size.Y, false, Image.Format.Rgba8);
            _fogImage.Fill(Colors.Black);

            if (_fogTexture == null)
            {
                _fogTexture = ImageTexture.CreateFromImage(_fogImage);
            }
            else
            {
                _fogTexture.Update(_fogImage);
            }
        }

        public void ApplyFogImage()
        {
            if (_fogImage == null)
            {
                GD.PrintErr("Fog image is not initialized before applying.");
                return;
            }

            if (_fogTexture == null)
            {
                _fogTexture = ImageTexture.CreateFromImage(_fogImage);
            }
            else
            {
                _fogTexture.Update(_fogImage);
            }
        }

        public void ApplyFogTexture(Vector2I position)
        {
            if (_fogTexture == null)
            {
                GD.PrintErr("Fog texture is not initialized before applying to sprite.");
                return;
            }

            _fogSprite.Texture = _fogTexture;
            Position = position;

            _fogRect.Size = (Vector2I)_fogTexture.GetSize();
            _fogRect.Position = position;
        }

        public void EraseFog(Image bitMap, Vector2I bitmapPosition)
        {
            Vector2I bitmapSize = bitMap.GetSize();
            Rect2I bitmapRect = new(bitmapPosition, bitmapSize);

            Rect2I intersectionRect = _fogRect.Intersection(bitmapRect);
            if (intersectionRect.Size == Vector2I.Zero)
            {
                GD.PrintErr("Bitmap is out of bounds");
                return;
            }

            Vector2I fogOffset = intersectionRect.Position - _fogRect.Position;
            Vector2I bitmapOffset = intersectionRect.Position - bitmapRect.Position;

            Image subFog = _fogImage.GetRegion(new Rect2I(fogOffset, intersectionRect.Size));
            Image subBitmap = bitMap.GetRegion(new Rect2I(bitmapOffset, intersectionRect.Size));

            byte[] fogData = subFog.GetData();
            byte[] eraseData = subBitmap.GetData();

            if (fogData.Length != eraseData.Length)
            {
                GD.PrintErr("Mismatch between fog and erase data size");
                return;
            }

            for (int i = 0; i < fogData.Length; i += 4)
            {
                byte fogAlpha = fogData[i + 3];
                byte eraseAlpha = eraseData[i + 3];

                if (fogAlpha == 0 || eraseAlpha == 0)
                {
                    fogData[i + 3] = 0;
                }
            }

            Image afterErase = Image.CreateFromData(intersectionRect.Size.X, intersectionRect.Size.Y, false, Image.Format.Rgba8, fogData);
            _fogImage.BlitRect(afterErase, new Rect2I(Vector2I.Zero, intersectionRect.Size), fogOffset);
            _fogTexture.Update(_fogImage);
        }

        public void SaveFogImage(string filePath)
        {
            if (_fogImage == null)
            {
                GD.PrintErr("Fog image is not exist, cannot save");
                return;
            }

            Error err = _fogImage.SavePng(filePath);
            if (err != Error.Ok)
            {
                GD.PrintErr($"Failed to save fog image to file: {filePath}; Error: {err}");
            }
        }

        public void LoadFogImage(string filePath)
        {
            _fogImage = new Image();
            Error err = _fogImage.Load(filePath);
            if (err != Error.Ok)
            {
                GD.PrintErr($"Failed to load fog image from file: {filePath}; Error: {err}");
            }
        }
    }
}
