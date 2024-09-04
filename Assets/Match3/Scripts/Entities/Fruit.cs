using DG.Tweening;
using Match3.Data;
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
            transform.DOScale(Vector3.one, duration: 0.3f)
                .SetEase(Ease.OutBack)
                .SetDelay(delay: Random.Range(0f, 0.1f));
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        public void SetData(FruitData data)
        {
            _data = data;
            _spriteRenderer.sprite = data.Sprite;
        }
    }
}
