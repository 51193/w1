using Godot;
using MyGame.Entity;
using MyGame.Entity.Data;
using System;
using System.Collections.Generic;

namespace MyGame.Strategy
{
    public abstract class BasicStrategy<T> : IStrategy where T : IEntity
    {
        public abstract List<Type> DataNeeded { get; }

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

        public D AccessData<D>(IEntity entity) where D : BasicData
        {
            return entity.DataManager.Get<D>();
        }
    }
}
