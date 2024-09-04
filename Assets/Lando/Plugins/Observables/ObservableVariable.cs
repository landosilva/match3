using System;
using UnityEngine;

namespace Lando.Plugins.Observables
{
    [Serializable]
    public abstract class ObservableVariable<TValueType> where TValueType : new()
    {
        [SerializeField] protected TValueType _value = new();
        
        public delegate void ObservableDelegate(TValueType value);
        public event ObservableDelegate OnChange;
        
        public virtual TValueType Value
        {
            get => _value;
            set => Set(value);
        }

        public abstract void Set(TValueType newValue);
        public virtual void Notify() => OnChange?.Invoke(_value);
    }
}
