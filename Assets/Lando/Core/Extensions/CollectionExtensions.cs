using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lando.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection == null || !collection.Any();
        
        public static T PickRandom<T>(this IEnumerable<T> collection)
        {
            if (!collection.Any())
                return default;
            
            int randomIndex = Random.Range(0, collection.Count());
            return collection.ElementAt(randomIndex);
        }
        
        public static T GetInBounds<T>(this IList<T> collection, ref int index)
        {
            if (index < 0)
                index = 0;
            else if (index >= collection.Count)
                index = collection.Count - 1;
            return collection[index];
        }
    }
}