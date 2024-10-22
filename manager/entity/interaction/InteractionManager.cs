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
			int index = 0;
			while(index < _interactableEntities.Count)
			{
				if (_interactableEntities[index].IsInteractableWith(_interactionParticipant))
				{
					_interactableEntities[index].ShowTip();
					return;
				}
				else
				{
					index++;
				}
			}
		}

		public void Interact()
        {
            int index = 0;
            while (index < _interactableEntities.Count)
            {
                if (_interactableEntities[index].IsInteractableWith(_interactionParticipant))
                {
                    _interactableEntities[index].Interact(_interactionParticipant);
                    return;
                }
                else
                {
                    index++;
                }
            }
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
