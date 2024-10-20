using Godot;
using MyGame.Component;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public class InteractionManager
	{
		private readonly IInteractionParticipant _interactionParticipant;
		private readonly List<IInteractableEntity> _interactableEntities = new();

		public InteractionManager(IInteractionParticipant participant)
		{
			_interactionParticipant = participant;
		}

		public void AddInteractableEntity(IInteractableEntity interactableEntity)
		{
			_interactableEntities.Add(interactableEntity);
		}

		public void RemoveInteractableEntity(IInteractableEntity interactableEntity)
		{
			interactableEntity.HideTip();
			_interactableEntities.Remove(interactableEntity);
		}

		public void ShowNearestTip()
        {
            if (_interactableEntities.Count == 0) return;
            _interactableEntities[0].ShowTip();
		}

		public void Interact()
		{
			if (_interactableEntities.Count == 0) return;
			_interactableEntities[0].Interact(_interactionParticipant);
		}

		public void ResortEntitiesOrder()
		{
            foreach (var entity in _interactableEntities)
            {
                entity.HideTip();
            }
            if (_interactableEntities.Count > 0)
            {
                _interactableEntities.Sort((a, b) =>
                {
                    return (_interactionParticipant.Position - a.Position).Length().CompareTo((_interactionParticipant.Position - b.Position).Length());
                });
            }
        }
	}
}
