using System.IO;
using UnityEditor;
using UnityEngine;

namespace Lando.Editor
{
    [CustomPropertyDrawer(typeof(SceneReferenceAttribute))]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUI.LabelField(position, label.text, label2: "Use [SceneReference] with int.");
                return;
            }
            
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            string[] sceneNames = new string[scenes.Length];
            int[] sceneIndices = new int[scenes.Length];

            for (int i = 0; i < scenes.Length; i++)
            {
                sceneNames[i] = Path.GetFileNameWithoutExtension(scenes[i].path);
                sceneIndices[i] = i;
            }
            
            int currentIndex = property.intValue;

            if (currentIndex < 0 || currentIndex >= scenes.Length) 
                currentIndex = 0; // Default to first scene if out of range
            
            int selectedIndex = EditorGUI.IntPopup(position, label.text, currentIndex, sceneNames, sceneIndices);
            
            property.intValue = selectedIndex;
        }
    }
}