using Lando.Plugins.Singletons.ScriptableObject;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Lando.Plugins.Singletons
{
    [CreateAssetMenu(fileName = "Bootstrap Objects", menuName = "Lando/Singletons/Bootstrap Objects")]
    public class BootstrapObjects : Singleton<BootstrapObjects>
    {
        [SerializeField] private GameObject[] _objects;

        public static void CreateObjects()
        {
            foreach (GameObject @object in Instance._objects) 
                CreateObject(@object);
        }

        private static void CreateObject(GameObject prefab)
        {
            GameObject instance = Instantiate(prefab);
            instance.name = prefab.name;
        }
    }
}