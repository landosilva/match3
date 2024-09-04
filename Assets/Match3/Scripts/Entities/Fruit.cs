using Match3.Data;
using PrimeTween;
using UnityEngine;

namespace Match3.Entities
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private FruitData _data;
        public string Identifier => _data.Identifier;
        public string Name => _data.name;

        private void OnEnable()
        {
            transform.localScale = Vector3.zero;
            TweenSettings settings = new()
            {
                duration = 0.3f,
                ease = Ease.OutBack,
                startDelay = Random.Range(0f, 0.15f)
            };
            Tween.Scale(transform, Vector3.one, settings);
        }

        public void SetData(FruitData data)
        {
            _data = data;
            _spriteRenderer.sprite = data.Sprite;
        }
    }
}
