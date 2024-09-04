using Lando.Plugins.Data;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceLitter.Data
{
    public class UniqueData : ScriptableObject
    {
        [SerializeField, ReadOnly] protected string _identifier;
        public string Identifier => _identifier;
        
        private void OnValidate()
        {
#if UNITY_EDITOR
            AttributeIdentifier();
#endif
        }
        
        private void OnEnable()
        {
            AttributeIdentifier();
        }

        public void AttributeIdentifier()
        {
#if UNITY_EDITOR
            EditorApplication.delayCall += DelayedCall;
            
            void DelayedCall()
            {
                string assetPath = AssetDatabase.GetAssetPath(this);
                string guid = AssetDatabase.AssetPathToGUID(assetPath);
                _identifier = guid;
                Database.TryAdd(data: this);
            }
#endif
        }
    }
}