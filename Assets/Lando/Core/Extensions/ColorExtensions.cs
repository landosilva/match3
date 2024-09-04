using UnityEngine;

namespace Lando.Core.Extensions
{
    public static class ColorExtensions
    {
        public static Color With(this Color color, float? r = null, float? g = null, float? b = null, float? a = null) 
            => new(r ?? color.r, g ?? color.g, b ?? color.b, a ?? color.a);
        
        public static string ToHex(this Color color) 
            => ColorUtility.ToHtmlStringRGB(color);
    }
}