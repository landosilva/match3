using Lando.Plugins.Singletons.MonoBehaviour;
using Match3.Entities;
using UnityEngine;

namespace Match3.Factories
{
    public class NodeFactory : Singleton<NodeFactory>
    {
        [SerializeField] private Node _nodePrefab;

        public static Node CreateNode(Vector2Int index, Vector3 position, Transform parent)
        {
            Node node = Instantiate(_instance._nodePrefab, parent);
            node.transform.position = position;
            node.Initialize(index);
            return node;
        }
    }
}