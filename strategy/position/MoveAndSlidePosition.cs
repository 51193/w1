using MyGame.Entity;

namespace MyGame.Strategy
{
    public class MoveAndSlidePosition : BasicStrategy<BasicDynamicEntity>
    {
        protected override void Activate(BasicDynamicEntity entity, double dt = 0)
        {
            entity.MoveAndSlide();
        }
    }
}
