using System.Collections.Generic;
using Lando.Core.Extensions;
using Lando.Plugins.Events;
using Match3.Factories;
using UnityEngine;
using Event = Lando.Plugins.Events.Event;

namespace Match3.Entities
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private Vector2Int _size;
        
        private readonly Dictionary<Vector2Int, Node> _map = new();
        private Dictionary<int, List<Node>> Columns { get; } = new();
        
        public Vector2Int Size => _size;
        public Vector3 Center => new(_size.x / 2f, _size.y / 2f, 0);

        private void Start()
        {
            CreateNodes();
        }

        public void CreateNodes()
        {
            _map.Clear();
            
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y * 2; y++)
                {
                    Vector2Int index = new(x, y);
                    IndexToWorld(index, out Vector3 worldPosition);
                    
                    Node node = NodeFactory.CreateNode(index, worldPosition, transform);
                    node.gameObject.SetActive(y < _size.y);
                    _map.Add(index, node);

                    if (!Columns.TryGetValue(x, out List<Node> column))
                    {
                        column = new List<Node>();
                        Columns.Add(x, column);
                    }
                    
                    column.Add(node);
                }
            }

            foreach (Node node in _map.Values) 
                node.SetNeighbours(grid: this);

            Events.Initialized onInitialized = new(grid: this);
            Event.Raise(onInitialized);
        }
        
        public bool TryGetNode(Vector2Int index, out Node result) => _map.TryGetValue(index, out result);
        private static void IndexToWorld(Vector2Int index, out Vector3 result) => result = index.With(z: 0);
        
        public static class Events
        {
            public class Initialized : IEvent
            {
                public Grid Grid { get; } 
                
                public Initialized(Grid grid)
                {
                    Grid = grid;
                }
            }
        }

        public void Fill()
        {
            foreach (Node node in _map.Values)
            {
                node.Clear();
                Fruit fruit = FruitFactory.CreateRandom();
                node.Place(fruit);
            }
        }
    }
}
