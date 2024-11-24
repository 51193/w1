using MyGame.Entity.Component;
using MyGame.Entity.Data;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public class DisplayNearestInteractableEntityTip : BasicStrategy<BasicPlayer>
    {
        public override List<Type> DataNeeded
        {
            get
            {
                return new List<Type>()
                {
                    typeof(NearestInteractableEntityData)
                };
            }
        }

        protected override void Activate(BasicPlayer entity, double dt = 0)
        {
            entity.AccessibleInteratableEntities.ForEach(e => { e.HideTip(); });
            AccessData<NearestInteractableEntityData>(entity).NearestEntity?.ShowTip();
        }
    }
}
