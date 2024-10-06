using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
	public partial class DoorOpenable : BaseInteractableStaticEntity
	{
		public override void _Ready()
		{
			InitInteractionPrompt(GetNode<Label>("Label"));
			_interactionStrategy = new LazyLoader<IInteractionStrategy>(() =>
			{
				return new OpenDoorStrategy(GetNode<AnimationPlayer>("AnimationPlayer"));
			});
		}
	}
}
