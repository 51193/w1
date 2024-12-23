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

        public void CreateFogTexture(Vector2I size)
        {
            _fogImage = Image.CreateEmpty(size.X, size.Y, false, Image.Format.Rgba8);
            _fogImage.Fill(Colors.Black);

            _fogTexture = ImageTexture.CreateFromImage(_fogImage);
        }

        public void ApplyFogTexture(Vector2I position)
        {
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


        public Image GenerateEraseBitmap(Vector2I center, int radius)
        {
            int size = radius * 2;
            Image bitmap = Image.CreateEmpty(size, size, false, Image.Format.Rgba8);
            bitmap.Fill(Colors.Black);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (center.DistanceTo(new Vector2I(x, y)) <= radius)
                    {
                        bitmap.SetPixel(x, y, new Color(0, 0, 0, 0));
                    }
                }
            }

            return bitmap;
        }
    }
}
