using Godot;
using MyGame.Entity;

namespace MyGame.Strategy
{
    public abstract class BasicStrategy<T> : IStrategy where T : IEntity
    {
        protected abstract void Activate(T entity, double dt = 0);

        public void Activate(IEntity entity, double dt = 0)
        {
            if (entity is T typedEntity)
            {
                Activate(typedEntity, dt);
            }
            else
            {
                GD.PrintErr($"{entity.EntityName} can't fit in type: {typeof(T).FullName} when activate strategy");
            }
        }
    }
}
