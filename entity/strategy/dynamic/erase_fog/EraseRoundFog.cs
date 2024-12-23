using Godot;
using MyGame.Entity.MainBody;
using MyGame.Manager;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public class EraseRoundFog : BasicStrategy<BasicDynamicEntity>
    {
        public override List<Type> DataNeeded
        {
            get
            {
                return new List<Type>();
            }
        }

        protected override void Activate(BasicDynamicEntity entity, double dt = 0)
        {
            FogManager.Instance.EraseCurrentFog(GenerateEraseBitmap(new Vector2I(100, 100), 100), (Vector2I)(entity.Position - new Vector2(100, 100)));
        }

        private static Image GenerateEraseBitmap(Vector2I center, int radius)
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
