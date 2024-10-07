using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
	public partial class InteractionTestEntity : BaseInteractableStaticEntity
	{
		public override void _Ready()
		{
			InitInteractionPrompt(GetNode<Label>("Label"));
            LoadStrategy(() =>
            {
				return new TestStrategy();
            });
        }
	}
}
