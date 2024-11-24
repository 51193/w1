using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public interface IStrategy
    {
        public List<Type> DataNeeded { get; }

        public void Activate(IEntity entity, double dt = 0);
    }
}
