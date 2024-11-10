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
            string callbackName = entity.CallbackOnTargetReached;

            if ((target - position).Length() < 10)
            {
                entity.Direction = Vector2.Zero;
                entity.EventManager.TriggerEvent(callbackName);
            }
            else
            {
                entity.Direction = (target - position).Normalized();
            }
        }
    }
}
