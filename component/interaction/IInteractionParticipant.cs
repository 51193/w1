using Godot;
using System.Collections.Generic;

namespace MyGame.Component
{
    public interface IInteractionParticipant
    {
        public Vector2 Position { get; set; }
        public bool CanRegistrateToInteractionManager();
        public HashSet<string> GetInteractionTags();
    }
}
