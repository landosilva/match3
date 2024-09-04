using System;
using System.Reflection;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Lando.Core.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool TryGetComponentInParent<T>(this Component source, out T component)
            => source.gameObject.TryGetComponentInParent<T>(out component);
        public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component)
        {
            component = gameObject.GetComponentInParent<T>();
            return component != null;
        }
        
        public static bool TryGetComponentInChildren<T>(this Component source, out T component)
            => source.gameObject.TryGetComponentInChildren<T>(out component);
        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component)
        {
            component = gameObject.GetComponentInChildren<T>();
            return component != null;
        }

        public static bool TryGetComponentEverywhere<T>(this Component source, out T component)
            => source.gameObject.TryGetComponentEverywhere(out component);
        public static bool TryGetComponentEverywhere<T>(this GameObject gameObject, out T component)
        {
            return gameObject.TryGetComponentInChildren(out component) || 
                   gameObject.TryGetComponentInParent(out component);
        }
        
        public static T GetOrAddComponent<T>(this Component component) where T : Component 
            => component.gameObject.GetComponent<T>() ?? component.gameObject.AddComponent<T>();

        private static T GetCopyOf<T>(this Component component, T other) where T : Component
        {
            Type type = component.GetType();
            if (type != other.GetType()) 
                return null;
            
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | 
                                       BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] properties = type.GetProperties(flags);
            foreach (PropertyInfo property in properties)
            {
                if (!property.CanWrite) 
                    continue;
                
                try { property.SetValue(component, property.GetValue(other, null), null); }
                catch(Exception e) { Debug.LogError(e.Message); }
            }
            
            FieldInfo[] fields = type.GetFields(flags);
            foreach (FieldInfo field in fields) 
                field.SetValue(component, field.GetValue(other));
            
            return component as T;
        }
        
        public static T AddComponentCopy<T>(this GameObject go, T toBeCopied) where T : Component 
            => go.AddComponent<T>().GetCopyOf(toBeCopied);

        public static void Activate(this Component component) => component.gameObject.SetActive(true);
        public static void Deactivate(this Component component) => component.gameObject.SetActive(false);
    }
}