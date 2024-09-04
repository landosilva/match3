using System;
using System.Collections.Generic;
using Lando.Core.Extensions;
using Lando.Plugins.Singletons.ScriptableObject;
using SpaceLitter.Data;
using UnityEngine;
# if UNITY_EDITOR
#endif

namespace Lando.Plugins.Data
{
    [CreateAssetMenu(fileName = "Database", menuName = "Lando/Data/Database")]
    public partial class Database : Singleton<Database>
    {
        [SerializeField, ReadOnly] private List<UniqueData> _data = new();
        [SerializeField] private string _filePath;
        
        private readonly Dictionary<Type, Dictionary<string, UniqueData>> _runtimeDataMap = new();

        public override void OnPreloaded() => CreateDataMap();

        private void CreateDataMap()
        {
            if (_data.IsNullOrEmpty())
                return;

            for (int i = _data.Count - 1; i >= 0; i--)
            {
                UniqueData data = _data[i];
                if (data == null)
                {
                    _data.RemoveAt(i);
                    continue;
                }
                if (!_runtimeDataMap.ContainsKey(data.GetType()))
                    _runtimeDataMap.Add(data.GetType(), new Dictionary<string, UniqueData>());
                _runtimeDataMap[data.GetType()].TryAdd(data.Identifier, data);
            }
        }
        
        public static void TryAdd(UniqueData data)
        {
            if (data == null || Instance._data.Contains(data))
                return;
            
            Instance._data.Add(data);
        }

        private static void TryRemove(UniqueData data)
        {
            if (data == null)
                return;
            
            Instance._data.Remove(data);
        }
        
        public static bool TryGetData<T>(string identifier, out T data) where T : UniqueData
        {
            data = default;
            if (!Instance._runtimeDataMap.TryGetValue(typeof(T), out Dictionary<string, UniqueData> dataMap))
                return false;
            if (!dataMap.TryGetValue(identifier, out UniqueData uniqueData))
                return false;
            data = (T) uniqueData;
            return true;
        }
        
        [Serializable]
        public class DataMap
        {
            public string Identifier { get; set; }
            public UniqueData UniqueData { get; set; }
        }
    }
}