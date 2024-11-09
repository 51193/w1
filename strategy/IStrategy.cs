using MyGame.Entity;

namespace MyGame.Strategy
{
    public interface IStrategy
    {
        public void Activate(IEntity entity);
    }
}
