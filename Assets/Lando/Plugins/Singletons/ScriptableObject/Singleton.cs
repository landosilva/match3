using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Lando.Core.Editor;
#endif

namespace Lando.Plugins.Singletons.ScriptableObject
{
    public abstract class Singleton : UnityEngine.ScriptableObject
    {
        protected virtual void OnEnable() => OnPreloaded();

        public abstract void OnPreloaded();
    }
    
    public abstract class Singleton<T> : Singleton where T : Singleton<T>
    {
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBeProtected.Global
        private static T _instance;

        protected static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance != null) 
                    return _instance;
                
                IEnumerable<T> list = PlayerSettings.GetPreloadedAssets().OfType<T>();
                if (!list.Any())
                {
                    string assetPath = AssetDatabase.FindAssets(filter: "t: " + typeof(T).Name).First();
                    _instance = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                }
                else
                    _instance = list.First();

                return _instance;
#endif
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        public override void OnPreloaded()
        {
#if UNITY_EDITOR
            PreloadedAssets.Add(this);
#endif
            Instance = (T) this;
        }
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class SingletonLoader
    {
        static SingletonLoader()
        {
            List<Object> preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            foreach (Singleton singleton in preloadedAssets.OfType<Singleton>()) 
                singleton.OnPreloaded();
        }
    }
#endif
}
