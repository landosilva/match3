using Lando.Core.Extensions;
using Lando.Plugins.Data;
using Lando.Plugins.Singletons.MonoBehaviour;
using Match3.Data;
using Match3.Entities;
using UnityEngine;

namespace Match3.Factories
{
    public class FruitFactory : Singleton<FruitFactory>
    {
        [SerializeField] private Fruit _fruitPrefab;

        public static Fruit CreateRandom()
        {
            string identifier = FruitIdentifier.Collection.PickRandom();
            return Create(identifier);
        }

        public static Fruit Create(string identifier)
        {
            Fruit fruit = Instantiate(_instance._fruitPrefab);
            fruit.name = _instance._fruitPrefab.name;

            if (Database.TryGetData(identifier, out FruitData data)) 
                fruit.SetData(data);
            
            return fruit;
        }
    }
}