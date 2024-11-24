using Godot;
using System;

namespace MyGame.Entity
{
	public partial class DoorOpenable : BasicInteractableStaticEntity
	{
		[Export]
		public AnimationPlayer AnimationPlayerNode;
	}
}
