using MyGame.Entity;
using MyGame.Entity.Data;
using System;
using System.Collections.Generic;

namespace MyGame.Strategy
{
    public interface IStrategy
    {
        public List<Type> DataNeeded { get; }

        public void Activate(IEntity entity, double dt = 0);
    }
}
