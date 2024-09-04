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
                for (int y = 0; y < _size.y; y++)
                {
                    Vector2Int index = new(x, y);
                    IndexToWorld(index, out Vector3 worldPosition);
                    
                    Node node = NodeFactory.CreateNode(index, worldPosition, transform);
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
        
        public async void Fill()
        {
            int attempts = 16;

            while (!TryGenerate() && attempts > 0)
            {
                attempts--;
                Debug.Log("Regenerating...");
            }
        }

        private bool TryGenerate()
        {
            foreach(Node node in _map.Values) 
                node.Clear();

            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    Vector2Int index = new(x, y);
                    if (!_map.TryGetValue(index, out Node node)) 
                        continue;
                        
                    string fruitIdentifier = GetValidFruitIdentifierForNode(node);
                    Fruit fruit = FruitFactory.Create(fruitIdentifier);
                    node.Place(fruit);
                }
            }

            return IsSolvable();
        }
        
        private string GetValidFruitIdentifierForNode(Node node)
        {   
            List<string> possibleFruits = new(FruitIdentifier.Collection);
        
            while (true)
            {
                string fruitIdentifier = possibleFruits.PickRandom();
                possibleFruits.Remove(fruitIdentifier);
                
                if (!AnyMatch())
                    return fruitIdentifier;
                
                continue;

                bool AnyMatch() => Check(Direction.Left) || Check(Direction.Down);
                bool Check(Vector2Int direction) => HasMatch(node, direction, fruitIdentifier);
            }
        }

        private bool IsSolvable()
        {
            const int minimumSolutions = 3;
            
            int solutions = 0;
            foreach (Node node in _map.Values)
            {
                if (CanSwapAndMatch(node, Direction.Up) || 
                    CanSwapAndMatch(node, Direction.Right))
                    solutions++;
            }

            Debug.Log("Possible solutions " + solutions);
            return solutions >= minimumSolutions;
        }

        private static bool CanSwapAndMatch(Node node, Vector2Int direction)
        {
            if (!node.TryGetNeighbour(direction, out Node neighbour)) 
                return false;
            
            Swap(origin: node, target: neighbour);
            bool hasMatch = HasMatch(neighbour, direction, neighbour.Fruit.Identifier) ||
                            HasMatch(node, -direction, node.Fruit.Identifier);
            Swap(origin: node, target: neighbour);

            return hasMatch;
        }

        private static void Swap(Node origin, Node target)
        {
            Fruit aux = origin.Fruit;
            origin.Place(target.Fruit);
            target.Place(aux);
        }

        private static bool HasMatch(Node node, Vector2Int direction, string identifier,  int checks = 2)
        {
            Node context = node;
            for (int i = 1; i <= checks; i++)
            {
                if (!context.TryGetNeighbour(direction, out Node neighbour))
                    return false;
                if (neighbour.Fruit == null || neighbour.Fruit.Identifier != identifier)
                    return false;
                context = neighbour;
            }
            return true;
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
    }
}
