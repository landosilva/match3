using System.Text.RegularExpressions;
using UnityEngine;

// ReSharper disable once UnusedMethodReturnValue.Global

namespace Lando.Core.Extensions
{
    public static class StringExtensions
    {
        private const string WHITESPACE_PATTERN = @"\s+";
        private const string NUMBER_PATTERN = @"\d";
        private const string SPECIAL_CHARACTER_PATTERN = @"\.";
        private static readonly Regex _whitespaceRegex = new(WHITESPACE_PATTERN);
        
        public static string ReplaceWhitespace(this string input, string replacement = "") 
            => _whitespaceRegex.Replace(input, replacement);
        public static string ReplaceNumbers(this string input, string replacement = "") 
            => Regex.Replace(input, NUMBER_PATTERN, replacement);
        
        public static string ReplaceSpecialCharacters(this string input, string replacement = "")
            => Regex.Replace(input, SPECIAL_CHARACTER_PATTERN, replacement);
        
        public static bool IsNullOrEmpty(this string text) 
            => string.IsNullOrEmpty(text);
        
        public static string ToColor(this string text, Color color) 
            => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        
        public static string ToBold(this string text) 
            => $"<b>{text}</b>";
        
        public static string MSpace(this string input, float space = 0.2f) => $"<mspace={space}em>{input}</mspace>";
    }
}
