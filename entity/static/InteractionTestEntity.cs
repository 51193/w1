using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
	public partial class InteractionTestEntity : BaseInteractableStaticEntity
	{
		public override void _Ready()
		{
			InitInteractionPrompt(GetNode<Label>("Label"));
			_interactionStrategy = new LazyLoader<IInteractionStrategy>(() =>
			{
				return new TestStrategy();
			});
		}
	}
}
