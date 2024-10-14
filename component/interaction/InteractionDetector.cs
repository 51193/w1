using Godot;
using MyGame.Manager;

namespace MyGame.Component
{
	public partial class InteractionDetector : Area2D
	{
		protected IInteractableEntity _parentInteractableEntity;

		protected void OnInteractableObjectEntered(Node node)
		{
			if(node is IInteractionParticipant participant && participant.CanRegistrateToInteractionManager() && _parentInteractableEntity.CanInteractWith(participant))
			{
				GlobalObjectManager.EmitRegistrateInteractablePairSignal(participant, _parentInteractableEntity);
			}
		}

		protected void OnInteractableObjectExited(Node node)
		{
			if (node is IInteractionParticipant participant && participant.CanRegistrateToInteractionManager() && _parentInteractableEntity.CanInteractWith(participant))
			{
				GlobalObjectManager.EmitUnregistrateInteractablePairSignal(participant, _parentInteractableEntity);
			}
		}

		public override void _Ready()
		{
			_parentInteractableEntity = GetParent<IInteractableEntity>();
            BodyEntered += OnInteractableObjectEntered;
            BodyExited += OnInteractableObjectExited;
        }
	}
}
