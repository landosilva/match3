using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Lando.Core.Editor
{
    public static class PreloadedAssets
    {   
        public static void Add(Object asset) 
        {
#if UNITY_EDITOR
            List<Object> preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();

            bool isAlreadyPreloaded = preloadedAssets.Any(IsEqualsToThis); 
            if(isAlreadyPreloaded)
                return;

            preloadedAssets.Add(asset);
            
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            
            return;

            bool IsEqualsToThis(Object preloadedAsset)
            {
                return preloadedAsset != null &&
                       preloadedAsset.GetInstanceID() == asset.GetInstanceID();
            }
#endif
        }
        
        public static void RemoveEmpties()
        {
#if UNITY_EDITOR
            Object[] preloadedAssets = NonNullPreloadedAssets();
            PlayerSettings.SetPreloadedAssets(preloadedAssets);
            
            return;

            Object[] NonNullPreloadedAssets() => PlayerSettings.GetPreloadedAssets().Where(NonNull).ToArray();
            bool NonNull(Object preloadedAsset) => preloadedAsset != null;
#endif
        }
    }
}