namespace MyGame.Entity.Data
{
    public class FaceDirectionTimerData : BasicData
    {
        public double FaceDirectionTransitCooldown { get; set; } = 0.1;
        public double FaceDirectionTransitTimer { get; set; } = 0;
    }
}
