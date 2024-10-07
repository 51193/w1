using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
	public partial class DoorOpenable : BaseInteractableStaticEntity
	{
		public override void _Ready()
		{
			InitInteractionPrompt(GetNode<Label>("Label"));
			LoadStrategy(() =>
			{
				return new OpenDoorStrategy(GetNode<AnimationPlayer>("AnimationPlayer"));
			});
		}
	}
}
