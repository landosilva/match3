# if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lando.Core.Editor;
using Lando.Core.Extensions;
using SpaceLitter.Data;
using UnityEditor;
using UnityEngine;

namespace Lando.Plugins.Data
{
    public partial class Database
    {
        public static void GenerateClass()
        {
            Dictionary<Type, ClassGenerator.Class> uniqueTypes = new();
            
            foreach (UniqueData data in Instance._data)
            {   
                if (!uniqueTypes.TryGetValue(data.GetType(), out ClassGenerator.Class category))
                {
                    category = new ClassGenerator.Class();
                    category.Name = data.GetType().Name.Replace("Data", "") + "Identifier";
                    category.Access = ClassGenerator.AccessModifier.Public;
                    category.IsStatic = true;
                    
                    category.Usings.Add("System.Collections.Generic");
                    
                    uniqueTypes.Add(data.GetType(), category);
                }
                
                ClassGenerator.Member entry = new();
                entry.Type = "string";
                entry.Name = data.name;
                entry.IsStatic = true;
                entry.IsReadonly = true;
                entry.Value = $"\"{data.Identifier}\"";
                
                category.Members.Add(entry);
            }

            foreach (ClassGenerator.Class value in uniqueTypes.Values)
            {
                ClassGenerator.Member list = new();
                list.Type = "List<string>";
                list.Name = "Collection";
                list.IsStatic = true;
                list.IsReadonly = true;
                list.Value = $"new List<string> {{ {string.Join(", ", value.Members.Select(member => member.Name))} }}";
                
                value.Members.Add(list);
            }
            
            string contents = string.Join("\n", uniqueTypes.Values.Select(Contents));
            string filePath = Path.Combine(Instance._filePath, $"{nameof(Database)}.Generated.cs");
            using StreamWriter writer = new(filePath);
            writer.Write(contents);
            writer.Close();
            
            AssetDatabase.Refresh();
            
            return;

            string Contents(ClassGenerator.Class @class) => @class.Contents;
        }
        
        public class DatabaseProcessor : AssetModificationProcessor
        {
            private static bool _isDuplicating;
            
            private static void OnWillCreateAsset(string path)
            {
                if (path.EndsWith(".asset")) 
                    _isDuplicating = true;
                
                if(_isDuplicating)
                    return;
                
                string cleanPath = path.Replace(".meta", "");
                
                EditorApplication.delayCall += DelayedCall;
                
                return;

                void DelayedCall()
                {
                    UniqueData uniqueData = AssetDatabase.LoadAssetAtPath<UniqueData>(cleanPath);
                    if (uniqueData == null) 
                        return;
                    uniqueData.AttributeIdentifier();
                }
            }

            private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
            {
                if (_isDuplicating)
                {
                    _isDuplicating = false;
                    return AssetDeleteResult.DidNotDelete;
                }
                
                UniqueData asset = AssetDatabase.LoadAssetAtPath<UniqueData>(assetPath);
                TryRemove(asset);
                return AssetDeleteResult.DidNotDelete;
            }
        }

        [CustomEditor(typeof(Database))]
        public class DatabaseEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                Database database = (Database) target;
                
                EditorGUI.BeginChangeCheck();
                
                DrawEntries();
                DrawTitle(text: "Generate Class");
                DrawFilePath();
                ButtonGenerateClass();
                
                if (EditorGUI.EndChangeCheck())
                    EditorUtility.SetDirty(target);

                return;

                void DrawEntries()
                {
                    foreach (UniqueData data in database._data)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField(data, typeof(UniqueData), allowSceneObjects: false);
                        EditorGUILayout.LabelField(data.Identifier);
                        EditorGUILayout.EndHorizontal();
                    }
                }
                
                void DrawTitle(string text, bool center = false)
                {
                    GUIStyle style = new(EditorStyles.boldLabel);
                    if (center)
                        style.alignment = TextAnchor.MiddleCenter;
                    EditorGUILayout.LabelField(text, style);
                }

                void DrawFilePath()
                {
                    EditorGUILayout.BeginHorizontal();
                        
                    GUI.enabled = false;
                    database._filePath = EditorGUILayout.TextField(database._filePath);
                    GUI.enabled = true;
                        
                    Texture folderIcon = EditorGUIUtility.FindTexture(name: "Folder Icon");
                        
                    if(database._filePath.IsNullOrEmpty())
                        database._filePath = Application.dataPath;
                        
                    if (GUILayout.Button(folderIcon, GUILayout.Width(32), GUILayout.Height(20)))
                        database._filePath = EditorUtility.SaveFolderPanel(title: "Select Folder", folder: Application.dataPath, defaultName: "");
                        
                    EditorGUILayout.EndHorizontal();
                }

                void ButtonGenerateClass()
                {
                    GUIContent buttonContent = new(text: "Generate Class", EditorGUIUtility.IconContent(name: "cs Script Icon").image);
                    GUILayoutOption buttonHeight = GUILayout.Height(20);
                    if (!GUILayout.Button(buttonContent, buttonHeight)) 
                        return;

                    GenerateClass();
                }
            }
        }
    }
}
#endif