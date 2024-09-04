using Lando.Core.Editor;
using UnityEditor;

namespace Lando.Plugins.Singletons.Editor
{
    public class SingletonPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets
            (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if(deletedAssets.Length > 0)
                PreloadedAssets.RemoveEmpties();
        }
    }
}