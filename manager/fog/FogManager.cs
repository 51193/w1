using Godot;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class FogManager: Node
	{
		[Export]
		private Sprite2D _fogSprite;

		private ImageTexture _fogTexture;
		private Image _fogImage;

		private Rect2I _fogRect;

		private void CreateFogTexture(Vector2I size)
		{
			_fogImage = Image.CreateEmpty(size.X, size.Y, false, Image.Format.Rgba8);
			_fogImage.Fill(Colors.Black);

			_fogTexture = ImageTexture.CreateFromImage(_fogImage);
		}

		private void ApplyFogTexture(Vector2I position)
		{
			_fogSprite.Texture = _fogTexture;
			_fogSprite.Position = position;

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

			intersectionRect.Position -= _fogRect.Position;
			Image subFog = _fogImage.GetRegion(intersectionRect);
			intersectionRect.Position += _fogRect.Position;

			intersectionRect.Position -= bitmapRect.Position;
			Image subBitmap = bitMap.GetRegion(intersectionRect);
			intersectionRect.Position += bitmapRect.Position;

			byte[] fogData = subFog.GetData();
			byte[] eraseData = subBitmap.GetData();

			int length = fogData.Length;

			for (int i = 0; i < length; i += 4)
			{
				byte fogAlpha = fogData[i + 3];
				byte eraseAlpha = eraseData[i + 3];

				if(fogAlpha == 0 ||  eraseAlpha == 0)
				{
					fogData[i + 3] = 0;
				}
			}

			Image afterErase = Image.CreateFromData(intersectionRect.Size.X, intersectionRect.Size.Y, false, Image.Format.Rgba8, fogData);
			_fogImage.BlitRect(afterErase, new Rect2I(Vector2I.Zero, intersectionRect.Size), intersectionRect.Position -= _fogRect.Position);
			_fogTexture.Update(_fogImage);
		}

		private Image GenerateEraseBitmap(Vector2I center, int radius)
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

		public override void _Ready()
		{
			Rect2I mapRect = new(new Vector2I(0, 0), new Vector2I(1024, 1024));
			CreateFogTexture(mapRect.Size);
			ApplyFogTexture(mapRect.Position);

			Image smallEraseBitmap = GenerateEraseBitmap(new(300, 300), 300);
			Vector2I smallBitmapPosition = new(0, 0);

			EraseFog(smallEraseBitmap, smallBitmapPosition);
		}
	}
}
