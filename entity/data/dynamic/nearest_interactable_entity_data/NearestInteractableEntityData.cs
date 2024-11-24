using MyGame.Entity.Component;

namespace MyGame.Entity.Data
{
    public class NearestInteractableEntityData : BasicData
    {
        public override bool IsSavable => false;
        public IInteractableEntity NearestEntity { get; set; }
    }
}
