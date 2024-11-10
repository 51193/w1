using MyGame.Entity;

namespace MyGame.Strategy
{
    public class FourFaceAnimation : BasicStrategy<BasicCharacter>
    {
        protected override void Activate(BasicCharacter entity, double dt = 0)
        {
            if(entity.Velocity.IsZeroApprox())
            {
                entity.AnimationName = "idle";
            }
            else
            {
                entity.AnimationName = "run";
            }
            entity.AnimatedSprite2DNode.Play($"{entity.AnimationName}-{entity.FaceDirection}");
        }
    }
}
