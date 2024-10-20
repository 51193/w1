using MyGame.Component;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Entity
{
    public abstract partial class BaseCharacter : BaseInteractableDynamicEntity, IInteractionParticipant
    {
        private readonly InteractionManager _interactionManager;

        public InteractionManager InteractionManager {  get { return _interactionManager; } }

        public BaseCharacter()
        {
            _interactionManager = new(this);
        }

        public override ISaveComponent SaveData(ISaveComponent saveComponent = null)
        {
            return HandleSaveData<CharacterSaveComponent>(saveComponent);
        }

        public override ISaveComponent LoadData(ISaveComponent saveComponent)
        {
            return HandleLoadData<CharacterSaveComponent>(saveComponent);
        }

        public abstract HashSet<string> GetInteractionTags();

        protected override void WhenPositionChange()
        {
            base.WhenPositionChange();
            InteractionManager.ResortEntitiesOrder();
        }
    }
}
