using Lando.Core.Extensions;
using UnityEditor;
using UnityEngine;

namespace Lando.Plugins.Generators.Editor
{
    public partial class Generator 
    {
#if UNITY_EDITOR
    [CustomEditor(typeof(Generator), editorForChildClasses: true)]
    public class GeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Generator generator = (Generator) target;
            
            DrawTitle(text: "Settings");
            
            EditorGUI.BeginChangeCheck();
            
            generator._className = EditorGUILayout.TextField(label: "Class Name", text: generator._className);
            generator._namespace = EditorGUILayout.TextField(label: "Namespace", text: generator._namespace);
            
            GUILayout.Space(pixels: 10);
            DrawTitle(text: "Generate Class");
            DrawFilePath();
            ButtonGenerateClass();
            
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(generator);

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
                generator._filePath = EditorGUILayout.TextField(generator.FilePath);
                GUI.enabled = true;
                    
                Texture folderIcon = EditorGUIUtility.FindTexture(name: "Folder Icon");
                    
                if(generator._filePath.IsNullOrEmpty())
                    generator._filePath = Application.dataPath;
                    
                GUILayoutOption[] options = {GUILayout.Width(32), GUILayout.Height(20)};
                if (GUILayout.Button(folderIcon, options))
                    generator._filePath = EditorUtility.SaveFolderPanel(title: "Select Folder", folder: Application.dataPath, defaultName: generator._filePath);
                    
                EditorGUILayout.EndHorizontal();
            }
            
            void ButtonGenerateClass()
            {
                GUIContent buttonContent = new(text: "Generate Class", EditorGUIUtility.IconContent(name: "cs Script Icon").image);
                GUILayoutOption buttonHeight = GUILayout.Height(20);
                if (!GUILayout.Button(buttonContent, buttonHeight)) 
                    return;

                generator.Generate();
                AssetDatabase.Refresh();
            }
        }
    }
#endif       
    }
}