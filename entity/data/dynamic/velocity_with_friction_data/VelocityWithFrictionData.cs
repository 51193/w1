namespace MyGame.Entity.Data
{
    public class VelocityWithFrictionData : BasicData
    {
        public float MaxVelocity { get; set; } = 100;
        public float Acceleration { get; set; } = 2000;
        public float Friction { get; set; } = 1000;
    }
}
