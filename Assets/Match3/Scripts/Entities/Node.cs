using System.Collections.Generic;
using UnityEngine;

namespace Match3.Entities
{
    public class Node : MonoBehaviour
    {
        private Fruit _fruit;
        
        private readonly Dictionary<Vector2Int, Node> _neighbours = new();
        
        public Vector2Int Index { get; set; }
        
        public void Initialize(Vector2Int index)
        {
            Index = index;
            name = $"Node ({index.x}, {index.y})";
        }
        
        public void SetNeighbours(Grid grid)
        {
            foreach (Vector2Int direction in Direction.All)
            {
                Vector2Int neighbourIndex = Index + direction;
                if (!grid.TryGetNode(neighbourIndex, out Node neighbour)) 
                    continue;
                
                _neighbours.Add(direction, neighbour);
            }
        }
        
        public void Place(Fruit fruit)
        {
            _fruit = fruit;
            fruit.transform.position = transform.position;
            fruit.gameObject.SetActive(gameObject.activeInHierarchy);
        }

        public void Clear()
        {
            if (_fruit == null) 
                return;
            
            Destroy(_fruit.gameObject);
            _fruit = null;
        }
    }
    
    public static class Direction
    {
        public static readonly Vector2Int Up = new(0, 1);
        public static Vector2Int Down => new(0, -1);
        private static Vector2Int Left => new(-1, 0);
        private static Vector2Int Right => new(1, 0);
            
        public static List<Vector2Int> All => new()
        {
            Up, Down, Left, Right,
        };
    }
}