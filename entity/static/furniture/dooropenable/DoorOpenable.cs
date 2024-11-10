using Godot;
using MyGame.Component;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public partial class DoorOpenable : BasicInteractableStaticEntity
	{
		[Export]
		public AnimationPlayer AnimationPlayerNode;
	}
}
