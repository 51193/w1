using System.Text.Json.Serialization;

namespace MyGame.Entity.Data
{
    public class AnimationTimelineData: BasicData
    {
        [JsonIgnore]
        public bool HaveInitialzed { get; set; } = false;
        public double CurrentAnimationPosition { get; set; } = 0;
    }
}
