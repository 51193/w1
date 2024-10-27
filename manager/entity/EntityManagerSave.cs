using Godot;
using MyGame.Component;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class EntityManager : Node
	{
		public EntityData GetEntityData()
		{
			EntityData entityData = new();
			foreach (var entityInfomation in _globalEntityInfomation)
			{
				List<EntityInstanceInfo> entityInstanceInfos = entityInfomation.Value;
				List<EntityInstanceInfoData> entityInstanceInfoDatas = new();
				foreach (var entityInstanceInfo in entityInstanceInfos)
				{
					entityInstanceInfoDatas.Add(new(entityInstanceInfo.EntityType, entityInstanceInfo.SaveHead.ToSaveComponentDataList()));
				}
				entityData.GlobalEntityInfoData[entityInfomation.Key] = entityInstanceInfoDatas;
			}
			return entityData;
		}
	}
}
