using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lando.Core.Editor;
using Lando.Core.Extensions;
using UnityEditor;
using UnityEngine;

namespace Lando.Plugins.Generators.Editor.Scene
{
    [CreateAssetMenu(fileName = "Scene Generator", menuName = "Lando/Generators/Scene")]
    public class SceneGenerator : Generator
    {
        protected override void Generate()
        {
            base.Generate();
            
            List<string> scenes = GetAllScenes();

            ClassGenerator.Class sceneClass = new()
            {
                Name = ClassName,
                Namespace = Namespace,
                Access = ClassGenerator.AccessModifier.Public,
                IsStatic = true
            };

            foreach (ClassGenerator.Member member in scenes.Select(AsMember))
                sceneClass.Members.Add(member);
            
            sceneClass.GenerateFile(FilePath, ClassName);

            return;

            ClassGenerator.Member AsMember(string scene) => new()
            {
                IsStatic = true,
                IsReadonly = true,
                Type = "int",
                Name = scene,
                Value = scenes.IndexOf(scene).ToString()
            };

            List<string> GetAllScenes()
            {
                return EditorBuildSettings.scenes.Select(SceneNames).ToList();

                string SceneNames(EditorBuildSettingsScene scene) =>
                    Path.GetFileNameWithoutExtension(scene.path)
                        .ReplaceWhitespace()
                        .ReplaceNumbers()
                        .ReplaceSpecialCharacters();
            }
        }
    }
}