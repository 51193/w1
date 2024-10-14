using Godot;
using System.Collections.Generic;

namespace MyGame.Component
{
    public interface IInteractableEntity
    {
        public Vector2 Position { get; set; }
        public HashSet<string> GetInteractableTags();
        public bool CanInteractWith(IInteractionParticipant participant)
        {
            return GetInteractableTags().Overlaps(participant.GetInteractionTags());
        }
        public void WhenParticipantIsNearest();
        public void WhenParticipantIsNotNearest();
        public void Interact(IInteractionParticipant participant);
    }
}
