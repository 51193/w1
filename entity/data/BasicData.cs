namespace MyGame.Entity.Data
{
    public abstract class BasicData
    {
        public int RefCount { get; set; } = 0;
        public virtual bool IsSavable => true;
    }
}
