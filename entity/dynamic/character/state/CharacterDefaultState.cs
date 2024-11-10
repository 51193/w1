using Godot;
using MyGame.Component;
using MyGame.Strategy;
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
            character.StrategyManager.AddStrategy<FourFaceDirection>(StrategyGroup.ProcessStrategy);
            character.StrategyManager.AddStrategy<FourFaceAnimation>(StrategyGroup.ProcessStrategy);

            character.StrategyManager.AddStrategy<DefaultVelocity>(StrategyGroup.PhysicsProcessStrategy);
            character.StrategyManager.AddStrategy<MoveAndSlidePosition>(StrategyGroup.PhysicsProcessStrategy);
        }
        public void OnExit(IEntity entity) { }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args) { }
    }
}
