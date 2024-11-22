using Godot;
using MyGame.Entity.Data;
using MyGame.Strategy;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Manager
{
    public class DataManager
    {
        public Dictionary<Type, BasicData> DataDict = new();

        public void AddData(Type type)
        {
            if (!DataDict.TryGetValue(type, out var data))
            {
                data = (BasicData)Activator.CreateInstance(type);
                DataDict[type] = data;
            }
            data.RefCount++;
        }

        public void RemoveData(Type type)
        {
            if (!DataDict.TryGetValue(type, out var data))
            {
                GD.PrintErr($"Fail to remove data {type.FullName}, because it's not exist");
                return;
            }

            data.RefCount--;

            if (data.RefCount == 0)
            {
                DataDict.Remove(type);
            }
        }

        public void LoadStrategyData(IStrategy strategy)
        {
            strategy.DataNeeded.ForEach(AddData);
        }

        public void UnloadStrategyData(IStrategy strategy)
        {
            strategy.DataNeeded.ForEach(RemoveData);
        }

        public T Get<T>() where T : BasicData
        {
            Type type = typeof(T);
            if (DataDict.TryGetValue(type, out var data))
            {
                return data as T;
            }
            GD.PrintErr($"Get a not exist data {type.FullName}");
            return null;
        }
    }
}
