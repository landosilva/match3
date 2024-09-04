using UnityEngine;

namespace Lando.Plugins.Generators.Editor
{   
    public partial class Generator : ScriptableObject
    {
        [SerializeField] private string _className = "Settings";
        [SerializeField] private string _filePath = "Scripts/Generated";
        [SerializeField] private string _namespace;

        protected string ClassName => _className;
        protected string FilePath => _filePath;
        protected string Namespace => _namespace;
        
        private bool HasNamespace => !string.IsNullOrEmpty(Namespace);

        protected virtual void Generate() { }
    }
}