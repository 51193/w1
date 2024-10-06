using Godot;
using MyGame.Entity;
using MyGame.Manager;

namespace MyGame.Component
{
	public partial class InteractionDetector : Area2D
	{
		protected BaseInteractableStaticEntity _parentInteractableStaticEntity;

		public InteractionDetector()
		{
			BodyEntered += OnInteractableObjectEntered;
			BodyExited += OnInteractableObjectExited;
		}

		protected void OnInteractableObjectEntered(Node node)
		{
			if(node is BaseInteractableDynamicEntity dynamicEntity)
			{
				GlobalObjectManager.EmitRegistrateInteractablePairSignal(dynamicEntity, _parentInteractableStaticEntity);
			}
		}
		protected void OnInteractableObjectExited(Node node)
		{
			if (node is BaseInteractableDynamicEntity dynamicEntity)
			{
				GlobalObjectManager.EmitUnregistrateInteractablePairSignal(dynamicEntity, _parentInteractableStaticEntity);
			}
		}

		public override void _Ready()
		{
			_parentInteractableStaticEntity = GetParent<BaseInteractableStaticEntity>();
		}
	}
}
