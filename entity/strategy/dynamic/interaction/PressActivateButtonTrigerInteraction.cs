using Godot;
using MyGame.Entity.Component;
using MyGame.Entity.Data;
using MyGame.Manager;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public class PressActivateButtonTrigerInteraction : BasicStrategy<BasicPlayer>
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
            if (Input.IsActionJustReleased("activate"))
            {
                IInteractableEntity nearestEntity = AccessData<NearestInteractableEntityData>(entity).NearestEntity;
                if(nearestEntity == null) return;
                nearestEntity.Interact();
            }
        }
    }
}
