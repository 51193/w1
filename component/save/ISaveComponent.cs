using MyGame.Entity;

namespace MyGame.Component
{
	public interface ISaveComponent
	{
		public ISaveComponent Next {  get; set; }
		public void SaveData(IEntity entity);
		public void LoadData(IEntity entity);
		public T SearchDataType<T>()
		{
			if(this is T t)
			{
				return t;
			}
			else if (Next is null)
			{
				return default;
			}
			else
			{
				return Next.SearchDataType<T>();
			}
		}
	}
}
