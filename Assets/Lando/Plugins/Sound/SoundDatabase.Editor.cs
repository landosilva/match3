using System.IO;
using Lando.Core.Extensions;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lando.Plugins.Sound
{
    public partial class SoundDatabase
    {
#if UNITY_EDITOR
        [CustomEditor(typeof(SoundDatabase))]
        public class SoundDatabaseEditor : Editor
        {   
            public override void OnInspectorGUI()
            {
                SoundDatabase database = Instance;
                
                EditorGUI.BeginChangeCheck();
                DrawCategories();
                
                GUILayout.Space(pixels: 10);
                DrawTitle(text: "Generate Class");
                DrawFilePath();
                ButtonGenerateClass();
                
                if (EditorGUI.EndChangeCheck())
                    EditorUtility.SetDirty(database);
                
                return;
                
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
                
                void DrawCategories()
                {
                    SerializedProperty categories = serializedObject.FindProperty(nameof(_categories));
                    EditorGUILayout.PropertyField(categories, includeChildren: true);
                    serializedObject.ApplyModifiedProperties();
                }

                void ButtonGenerateClass()
                {
                    GUIContent buttonContent = new(text: "Generate Class", EditorGUIUtility.IconContent(name: "cs Script Icon").image);
                    GUILayoutOption buttonHeight = GUILayout.Height(20);
                    if (!GUILayout.Button(buttonContent, buttonHeight)) 
                        return;

                    GenerateClass();
                    AssetDatabase.Refresh();
                }
            }
        }
        
        private static void GenerateClass()
        {
            SoundDatabase database = Instance;
            string filePath = Path.Combine(database._filePath, $"{nameof(SoundDatabase)}.Generated.cs");

            using StreamWriter writer = new(filePath);
            
            writer.WriteLine("using UnityEngine;");
            writer.WriteLine();
            writer.WriteLine($"namespace {database.GetType().Namespace}");
            writer.WriteLine("{");
            writer.WriteLine("    public partial class SoundDatabase");
            writer.WriteLine("    {");

            for (int categoryIndex = 0; categoryIndex < database._categories.Length; categoryIndex++)
            {
                AudioCategory category = database._categories[categoryIndex];
                writer.WriteLine($"        public static class {category.Identifier}");
                writer.WriteLine("        {");

                for (int audioIndex = 0; audioIndex < category.Audios.Length; audioIndex++)
                {
                    AudioData audio = category.Audios[audioIndex];
                    writer.WriteLine($"            public class {audio.Identifier}Data");
                    writer.WriteLine("            {");

                    for (int clipIndex = 0; clipIndex < audio.Clips.Length; clipIndex++)
                    {
                        if (audio.Clips[clipIndex] == null)
                            continue;
                        
                        AudioClip clip = audio.Clips[clipIndex];
                        writer.WriteLine(
                            $"                public static AudioClip {clip.name.ReplaceWhitespace()} => _instance._categories[{categoryIndex}].Audios[{audioIndex}].Clips[{clipIndex}];");
                    }

                    WriteImplicitOperator();

                    writer.WriteLine("            }");
                    writer.WriteLine($"            public static {audio.Identifier}Data {audio.Identifier} => new {audio.Identifier}Data();");
                    
                    continue;
                    
                    void WriteImplicitOperator()
                    {
                        writer.WriteLine();
                        writer.WriteLine($"                public static implicit operator AudioData({audio.Identifier}Data _) =>\n" +
                                         $"                    _instance._categories[{categoryIndex}].Audios[{audioIndex}];");
                    }
                }

                writer.WriteLine("        }");
            }
            
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.Close();
        }
#endif       
    }
}