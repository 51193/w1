using MyGame.Component;
namespace MyGame.Entity
{
    public class CharacterDefaultState : IState
    {
        public void OnEnter(IEntity entity)
        {
            ((DynamicEntity0)entity).LoadStrategy(() =>
            {
                return new CharacterAnimationPlayer(((DynamicEntity0)entity).AnimationSprite2DNode);
            });
            ((DynamicEntity0)entity).LoadStrategy(() =>
            {
                return new FrictionVelocityAlgorithm(((DynamicEntity0)entity), 100, 2000, 1000);
            });
        }
        public void OnExit(IEntity entity) { }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args) { }
    }
}
