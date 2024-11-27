using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.Entity.Manager
{
    public class DataManager
    {
        private readonly Dictionary<Type, BasicData> _dataDict = new();
        public Dictionary<Type, BasicData> DataDict
        {
            get
            {
                Dictionary<Type, BasicData> dict = 
                    _dataDict.Where(kvp => kvp.Value.IsSavable).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                foreach (var data in dict.Values)
                {
                    data.ResetFlags();
                }
                return dict;
            }
            set
            {
                foreach (var item in value)
                {
                    if(_dataDict.ContainsKey(item.Key))
                    {
                        GD.Print($"{item.Key.FullName} is overrided by data loading");
                        _dataDict[item.Key] = item.Value;
                    }
                }
            }
        }

        public void AddData(Type type)
        {
            if (!_dataDict.TryGetValue(type, out var data))
            {
                GD.Print($"{type.FullName} added to DataManager");
                data = (BasicData)Activator.CreateInstance(type);
                _dataDict[type] = data;
            }
            data.RefCount++;
        }

        public void RemoveData(Type type)
        {
            if (!_dataDict.TryGetValue(type, out var data))
            {
                GD.PrintErr($"Fail to remove data {type.FullName}, because it's not exist");
                return;
            }

            data.RefCount--;

            if (data.RefCount == 0)
            {
                _dataDict.Remove(type);
                GD.Print($"{type.FullName} removed from DataManager");
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
            if (_dataDict.TryGetValue(type, out var data))
            {
                return data as T;
            }
            GD.PrintErr($"Get a not exist data {type.FullName}");
            return null;
        }

        public T TryGet<T>() where T : BasicData
        {
            Type type = typeof(T);
            if (_dataDict.TryGetValue(type, out var data))
            {
                return data as T;
            }
            return null;
        }
    }
}
