using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
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
