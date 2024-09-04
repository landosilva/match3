using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lando.Core.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null) 
            => new(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        public static Vector3 With(this Vector2 vector, float? x = null, float? y = null, float? z = null) 
            => new(x ?? vector.x, y ?? vector.y, z ?? 0);
        public static Vector3 With(this Vector2Int vector, float? x = null, float? y = null, float? z = null) 
            => new(x ?? vector.x, y ?? vector.y, z ?? 0);
        
        public static Vector2 Add(this Vector2Int vector, Vector2 other) 
            => new(vector.x + other.x, vector.y + other.y);
        public static Vector3 Add(this Vector3 vector, Vector3 other)
            => new(vector.x + other.x, vector.y + other.y, vector.z + other.z);
        
        public static Vector3 Multiply(this Vector3 vector, float multiplier)
            => new(vector.x * multiplier, vector.y * multiplier, vector.z * multiplier);
        
        public static Vector2Int Divide(this Vector2Int vector, int divisor) 
            => (vector.ToFloat() / divisor).RoundToInt();
        
        public static Vector2Int RoundToInt(this Vector3 vector)
            => new(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
        public static Vector2Int RoundToInt(this Vector2 vector)
            => new(Mathf.FloorToInt(vector.x), Mathf.RoundToInt(vector.y));
        
        public static Vector2 ToFloat(this Vector2Int vector)
            => new(vector.x, vector.y);
        
        public static Vector3 Center(this List<Vector3> vectors)
        {
            Vector3 result = vectors.Aggregate(Vector3.zero, Sum);

            return result / vectors.Count;

            Vector3 Sum(Vector3 current, Vector3 vector) => current + vector;
        }
    }
}