using System.Collections.Generic;
using System.IO;
using System.Text;
using Lando.Core.Utilities;
using UnityEditor;

namespace Lando.Core.Editor
{
    public static class ClassGenerator
    {
        public enum AccessModifier
        {
            Public,
            Private,
            Protected,
            Internal
        }
        
        private static string GetString(AccessModifier accessModifier) => accessModifier switch
        {
            AccessModifier.Public => "public",
            AccessModifier.Private => "private",
            AccessModifier.Protected => "protected",
            AccessModifier.Internal => "internal",
            _ => "public"
        };
        
        public class Member
        {
            public string Type;
            public string Name;
            public AccessModifier Access = AccessModifier.Public;
            public bool IsStatic;
            public bool IsReadonly;
            public bool IsConst;
            public bool IsLambda;
            public string Value;
            
            public string Contents
            {
                get
                {
                    StringBuilder stringBuilder = new();
                    
                    stringBuilder.Append($"{GetString(Access)}{(IsStatic ? " static" : "")}{(IsReadonly ? " readonly" : "")}{(IsConst ? " const" : "")} {Type} {Name}");
                    
                    string equals = IsLambda ? "=>" : "=";
                    if (!string.IsNullOrEmpty(Value))
                        stringBuilder.Append($" {equals} {Value}");
                    
                    stringBuilder.Append(";");
                    
                    return stringBuilder.ToString();
                }
            }
        }
        
        public class Class
        {
            public string Namespace;
            public AccessModifier Access = AccessModifier.Public;
            public bool IsPartial;
            public bool IsStatic;
            public string Name;
            public readonly List<string> Usings = new();
            public readonly List<Member> Members = new();
            public readonly List<Class> Classes = new();

            private int _identLevel;

            public string Contents
            {
                get
                {
                    StringBuilder stringBuilder = new();
                    
                    foreach (string @using in Usings)
                        AddLine($"using {@using};");
                    
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        AddLine($"namespace {Namespace}");
                        OpenCurlyBrackets();
                    }
                    
                    AddLine($"{GetString(Access)}{(IsPartial ? " partial" : "")}{(IsStatic ? " static" : "")} class {Name}");
                    OpenCurlyBrackets();
                    
                    foreach (Member member in Members)
                        AddLine(member.Contents);
                    
                    if (Classes.Count > 0)
                        AddLine();

                    foreach (Class @class in Classes)
                    {
                        int indentLevel = _identLevel;
                        _identLevel = 0;
                        @class._identLevel = indentLevel;
                        AddLine(@class.Contents);
                        _identLevel = indentLevel;
                    }
                    
                    CloseCurlyBrackets();
                    
                    if (!string.IsNullOrEmpty(Namespace))
                        CloseCurlyBrackets();
                    
                    return stringBuilder.ToString();
                    
                    string Ident(int level) => StringUtilities.Indent(level);
                    void OpenCurlyBrackets() => stringBuilder.AppendLine($"{Ident(_identLevel++)}{{");
                    void CloseCurlyBrackets() => stringBuilder.AppendLine($"{Ident(--_identLevel)}}}");
                    void AddLine(string line = "") => stringBuilder.AppendLine($"{Ident(_identLevel)}{line}");
                }
            }
            
            public void GenerateFile(string filePath, string fileName)
            {
                string fullFilePath = Path.Combine(filePath, $"{fileName}.cs");
                using StreamWriter writer = new(fullFilePath);
                writer.Write(Contents);
                writer.Close();
                AssetDatabase.Refresh();
            }
        }
    }
}