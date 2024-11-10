using Godot;
using MyGame.Entity;

namespace MyGame.Strategy
{
    public class StraightForwardDirection : BasicStrategy<BasicDynamicEntity>
    {
        protected override void Activate(BasicDynamicEntity entity, double dt = 0)
        {
            Vector2 position = entity.Position;
            Vector2 target = entity.TargetPosition;
            EventContainer callback = entity.CallbackOnTargetReached;

            if ((target - position).Length() < 10)
            {
                entity.Direction = Vector2.Zero;
                callback.ActivateEvents(entity);
            }
            else
            {
                entity.Direction = (target - position).Normalized();
            }
        }
    }
}
