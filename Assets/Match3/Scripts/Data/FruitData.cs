using SpaceLitter.Data;
using UnityEngine;

namespace Match3.Data
{
    [CreateAssetMenu(fileName = "Fruit Data", menuName = "Match3/Data/Fruit")]
    public class FruitData : UniqueData
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}
