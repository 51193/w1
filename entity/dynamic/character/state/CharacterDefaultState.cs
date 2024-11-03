using Godot;
using MyGame.Component;
using MyGame.Util;
namespace MyGame.Entity
{
    public class CharacterDefaultState : IState
    {
        private Ref<string> _animationPlayed = new();
        public string AnimationPlayed
        {
            get
            {
                return _animationPlayed.Value;
            }
            set
            {
                _animationPlayed.Value = value;
            }
        }

        public void OnEnter(IEntity entity)
        {
            BasicCharacter character = entity as BasicCharacter;
            character.LoadStrategy(() =>
            {
                return new CharacterAnimationPlayer(character.AnimationSprite2DNode, _animationPlayed);
            });
            character.LoadStrategy(() =>
            {
                return new FrictionVelocityAlgorithm(character, 100, 2000, 1000);
            });
        }
        public void OnExit(IEntity entity) { }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args) { }
    }
}
