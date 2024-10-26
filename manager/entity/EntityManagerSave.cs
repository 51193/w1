using Godot;
using MyGame.Component;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class EntityManager : Node
	{
		public static List<SaveComponentNodeData> ToSaveComponentNodeDataList(ISaveComponent saveHead)
		{
			List<SaveComponentNodeData> nodes = new();

			ISaveComponent cur = saveHead;
			while (cur != null)
			{
				SaveComponentNodeData node = new
					(
						cur.GetType().FullName,
						new()
					);

				foreach (var field in cur.GetType().GetFields())
				{
					if (field.Name == "Next") continue;
					node.Properties[field.Name] = field.GetValue(cur);
				}

				nodes.Add(node);
				cur = cur.Next;
			}

			return nodes;
		}

		public static ISaveComponent FromSaveComponentNodeDataList(List<SaveComponentNodeData> nodes)
		{
			ISaveComponent saveHead = null;
			ISaveComponent cur = null;

			foreach (var node in nodes)
			{
				Type nodeType = Type.GetType(node.Type);

                if (nodeType == null)
				{
					GD.PrintErr($"Type '{node.Type}' could not be found when convert from save component node data list");
					return null;
                }

                var instance = (ISaveComponent)Activator.CreateInstance(nodeType);

                foreach (var property in node.Properties)
                {
                    var propertyInfo = nodeType.GetProperty(property.Key);
					if (propertyInfo != null)
					{
						propertyInfo.SetValue(instance, property.Value);
					}
					else
					{
						GD.PrintErr($"Property '{property.Key}' not found on type '{nodeType.FullName}' when convert from save component node data list");
						return null;                    
					}
				}

				if(saveHead == null)
				{
					saveHead = instance;
				}
				else
				{
					cur.Next = instance;
				}
				cur = instance;
			}

			return saveHead;
		}

		public EntitySaveData GetEntitySaveData()
		{
			EntitySaveData entitySaveData = new();
			foreach (var entityInfomation in _globalEntityInfomation)
			{
				List<EntityInstanceInfo> entityInstanceInfos = entityInfomation.Value;
				List<EntityInstanceInfoData> entityInstanceInfoDatas = new();
				foreach (var entityInstanceInfo in entityInstanceInfos)
				{
					entityInstanceInfoDatas.Add(new(entityInstanceInfo.EntityType, ToSaveComponentNodeDataList(entityInstanceInfo.SaveHead)));
				}
				entitySaveData.GlobalEntityInfoData[entityInfomation.Key] = entityInstanceInfoDatas;
			}
			return entitySaveData;
		}

		public void ApplyEntitySaveData(EntitySaveData entitySaveData)
		{
			_globalEntityInfomation.Clear();
			foreach (var entityInfoData in entitySaveData.GlobalEntityInfoData)
			{
				List<EntityInstanceInfoData> entityInstanceInfoDatas = entityInfoData.Value;
				List<EntityInstanceInfo> entityInstanceInfos = new();
				foreach(var entityInstanceInfoData in entityInstanceInfoDatas)
				{
					entityInstanceInfos.Add(new(entityInstanceInfoData.EntityType, FromSaveComponentNodeDataList(entityInstanceInfoData.SaveNodesList)));
				}
				_globalEntityInfomation[entityInfoData.Key] = entityInstanceInfos;
			}
		}
	}
}
