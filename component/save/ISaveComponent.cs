using Godot;
using MyGame.Entity;
using System.Collections.Generic;

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

		public SaveComponentData ToSaveComponentData()
		{
			SaveComponentData saveComponentData = new()
			{
				SaveComponentType = GetType().FullName
			};

            foreach (var property in GetType().GetProperties())
            {
                if (property.Name == nameof(Next)) continue;
				saveComponentData.Properties[property.Name] = property.GetValue(this);
            }

            return saveComponentData;
		}

		public List<SaveComponentData> ToSaveComponentDataList()
		{
			List<SaveComponentData> saveComponentDataList = new();
			ISaveComponent cur = this;
			while (cur != null)
			{
				saveComponentDataList.Add(cur.ToSaveComponentData());
				cur = cur.Next;
			}
			return saveComponentDataList;
		}
	}
}
