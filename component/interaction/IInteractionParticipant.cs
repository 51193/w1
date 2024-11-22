using Godot;
using MyGame.Entity.Manager;
using System.Collections.Generic;

namespace MyGame.Component
{
    public interface IInteractionParticipant
    {
        public InteractionManager InteractionManager { get; init; }
        public Vector2 Position { get; set; }
        public HashSet<string> GetInteractionTags();
    }
}
