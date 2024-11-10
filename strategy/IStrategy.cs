using MyGame.Entity;
using System;

namespace MyGame.Strategy
{
    public interface IStrategy
    {
        public void Activate(IEntity entity, double dt = 0);
    }
}
