using Godot;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Component
{
    public interface IInteractionParticipant
    {
        public InteractionManager InteractionManager { get; }
        public Vector2 Position { get; set; }
        public HashSet<string> GetInteractionTags();
    }
}
