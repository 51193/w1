using Godot;
using MyGame.Component;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public partial class AnotherInteractionTestEntity : BaseInteractableStaticEntity
	{
		public override HashSet<string> GetInteractableTags()
		{
			return new HashSet<string>();
		}

        public override void WhenParticipantIsNearest()
        {
        }

        public override void WhenParticipantIsNotNearest()
        {
        }

        public override void _Ready()
		{
			LoadStrategy(() =>
			{
				return new TestStrategy();
			});
		}
	}
}
