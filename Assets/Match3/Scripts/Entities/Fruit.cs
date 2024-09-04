using Match3.Data;
using UnityEngine;

namespace Match3.Entities
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private FruitData _data;
        public string Identifier => _data.Identifier;

        public void SetData(FruitData data)
        {
            _data = data;
            _spriteRenderer.sprite = data.Sprite;
        }
    }
}
