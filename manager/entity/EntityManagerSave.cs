using Godot;
using MyGame.Component;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class EntityManager : Node
	{
		public Dictionary<string, List<EntityInstanceInfoData>> ToEntityInstanceInfoDataDictionary()
		{
			Dictionary<string, List<EntityInstanceInfoData>> entityInstanceInfoDataDictionary = new();
			foreach (var entityInfomation in _globalEntityInfomation)
			{
				List<EntityInstanceInfo> entityInstanceInfos = entityInfomation.Value;
				List<EntityInstanceInfoData> entityInstanceInfoDatas = new();
				foreach (var entityInstanceInfo in entityInstanceInfos)
				{
					entityInstanceInfoDatas.Add(new(entityInstanceInfo.EntityType, entityInstanceInfo.SaveHead.ToSaveComponentDataList()));
				}
				entityInstanceInfoDataDictionary[entityInfomation.Key] = entityInstanceInfoDatas;
			}
			return entityInstanceInfoDataDictionary;
		}

		public void FromEntityInstanceInfoDataDictionary(Dictionary<string, List<EntityInstanceInfoData>> entityInstanceInfoDataDictionary)
		{
			_globalEntityInfomation.Clear();
			foreach (var kvp in entityInstanceInfoDataDictionary)
			{
				List<EntityInstanceInfo> entityInstanceInfoList = new();
				foreach(var entityInstanceInfoData in kvp.Value)
				{
					ISaveComponent saveHead = FromSaveComponentDataList(entityInstanceInfoData.SaveNodeList);
					EntityInstanceInfo entityInstanceInfo = new(entityInstanceInfoData.EntityType, saveHead);
					entityInstanceInfoList.Add(entityInstanceInfo);
				}
				_globalEntityInfomation[kvp.Key] = entityInstanceInfoList;
			}
		}

		private static ISaveComponent FromSaveComponentDataList(List<SaveComponentData> saveComponentDataList)
		{
			ISaveComponent head = null;
			ISaveComponent cur = null;

            foreach (var saveComponentData in saveComponentDataList)
            {
                Type type = Type.GetType(saveComponentData.SaveComponentType);
				if (type == null)
				{
					GD.PrintErr($"Type '{saveComponentData.SaveComponentType}' not found");
					return null;
				}

				ISaveComponent instance = (ISaveComponent)Activator.CreateInstance(type);

				foreach (var kvp in saveComponentData.Properties)
				{
					var property = type.GetProperty(kvp.Key);
					if (property == null)
					{
						GD.PrintErr($"Property '{kvp.Key}' not found in {type.FullName}");
						return null;
					}
					property.SetValue(instance, kvp.Value);
				}

				if (head == null)
				{
					head = instance;
				}
				else
				{
					cur.Next = instance;
				}

				cur = instance;
			}

			return head;
		}
	}
}
