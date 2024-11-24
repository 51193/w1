using MyGame.Entity.Data;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public class DoorInteraction : BasicStrategy<DoorOpenable>
    {
        public override List<Type> DataNeeded
        {
            get
            {
                return new List<Type>()
                {
                    typeof(InteractionStrategyTypeData)
                };
            }
        }

        protected override void Activate(DoorOpenable entity, double dt = 0)
        {
            entity.StateManager.Transit("OpenState", "");
        }
    }
}
