using MyGame.Entity;
using System;
using System.Collections.Generic;

namespace MyGame.Strategy
{
    public class MoveAndSlidePosition : BasicStrategy<BasicDynamicEntity>
    {
        public override List<Type> DataNeeded
        {
            get
            {
                return new List<Type>() { };
            }
        }

        protected override void Activate(BasicDynamicEntity entity, double dt = 0)
        {
            entity.MoveAndSlide();
        }
    }
}
