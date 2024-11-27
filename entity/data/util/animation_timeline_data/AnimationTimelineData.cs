using System.Text.Json.Serialization;

namespace MyGame.Entity.Data
{
    public class AnimationTimelineData: BasicData
    {
        [JsonIgnore]
        public bool HaveInitialized { get; set; } = false;
        public double CurrentAnimationPosition { get; set; } = 0;

        public override void ResetFlags()
        {
            HaveInitialized = false;
        }
    }
}
