using System.Collections.Generic;
using System.Linq;
using Lando.Core.Editor;
using Lando.Core.Extensions;
using UnityEngine;

namespace Lando.Plugins.Generators.Editor.Layer
{
    [CreateAssetMenu(fileName = "Layer Generator", menuName = "Lando/Generators/Layer")]
    public class LayerGenerator : Generator
    {
        protected override void Generate()
        {
            base.Generate();
            
            ClassGenerator.Class layerClass = new()
            {
                Name = ClassName,
                Namespace = Namespace,
                Access = ClassGenerator.AccessModifier.Public,
                IsStatic = true
            };
            
            ClassGenerator.Class maskClass = new()
            {
                Name = "Mask",
                Access = ClassGenerator.AccessModifier.Public,
                IsStatic = true
            };
            
            List<LayerData> layersData = GetAllLayersData();

            foreach (ClassGenerator.Member member in layersData.Select(LayerAsMember)) 
                layerClass.Members.Add(member);

            foreach (ClassGenerator.Member member in layersData.Select(MaskAsMember))
                maskClass.Members.Add(member);
            
            layerClass.Classes.Add(maskClass);
            layerClass.GenerateFile(FilePath, ClassName);
            
            return;

            ClassGenerator.Member LayerAsMember(LayerData layerData) =>
                new()
                {
                    IsStatic = true,
                    IsReadonly = true,
                    Type = "int",
                    Name = layerData.Name,
                    Value = layerData.Layer.ToString()
                };
            
            ClassGenerator.Member MaskAsMember(LayerData layerData) =>
                new()
                {
                    IsStatic = true,
                    IsReadonly = true,
                    Type = "int",
                    Name = layerData.Name,
                    Value = $"1 << {layerData.Layer}"
                };
        }

        private static List<LayerData> GetAllLayersData()
        {
            List<LayerData> layers = new();
            
            for (int layer = 0; layer < 31; layer++)
            {
                LayerData layerData = new (layer);
                if(layerData.IsValid)
                    continue;
                
                layers.Add(layerData);
            }
            
            return layers;
        }

        private class LayerData
        {
            public readonly string Name;
            public readonly int Layer;

            private readonly string _originalName;
            
            public bool IsValid => string.IsNullOrEmpty(_originalName);
            
            internal LayerData(int layer)
            {
                _originalName = LayerMask.LayerToName(layer);
                Name = _originalName.ReplaceWhitespace();
                Layer = layer;
            }
        }
    }
}
