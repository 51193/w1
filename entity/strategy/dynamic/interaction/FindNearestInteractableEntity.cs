using MyGame.Entity.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.Entity.Strategy
{
    public class FindNearestInteractableEntity : BasicStrategy<BasicPlayer>
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
            AccessData<NearestInteractableEntityData>(entity).NearestEntity = entity.AccessibleInteratableEntities.MinBy(a => (a.Position - entity.Position).Length());
        }
    }
}
