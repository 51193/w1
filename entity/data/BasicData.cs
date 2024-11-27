using System.Text.Json.Serialization;

namespace MyGame.Entity.Data
{
    public abstract class BasicData
    {
        public int RefCount { get; set; } = 0;
        [JsonIgnore]
        public virtual bool IsSavable => true;

        public virtual void ResetFlags() { }
    }
}
