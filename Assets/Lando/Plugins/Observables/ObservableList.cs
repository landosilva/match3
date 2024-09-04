using System;
using System.Collections;
using System.Collections.Generic;

namespace Lando.Plugins.Observables
{
    [Serializable]
    public class ObservableList<T> : ObservableVariable<List<T>>, IEnumerable<T>
    {
        public T this[int index]
        {
            get => Value[index];
            set
            {
                Value[index] = value;
                Notify();
            }
        }
        
        public ObservableList(List<T> value = null) 
            => Set(value ?? new List<T>());

        public sealed override void Set(List<T> newValue)
        {
            _value = newValue;
            Notify();
        }

        public void Add(T item)
        {
            Value.Add(item);
            Notify();
        }

        public void Remove(T item)
        {
            Value.Remove(item);
            Notify();
        }

        public void RemoveAt(int index)
        {
            Value.RemoveAt(index);
            Notify();
        }

        public void Clear()
        {
            Value.Clear();
            Notify();
        }

        public void AddRange(IEnumerable<T> collection)
        {
            Value.AddRange(collection);
            Notify();
        }

        public void Insert(int index, T item)
        {
            Value.Insert(index, item);
            Notify();
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            Value.InsertRange(index, collection);
            Notify();
        }

        public void RemoveRange(int index, int count)
        {
            Value.RemoveRange(index, count);
            Notify();
        }

        public static implicit operator List<T>(ObservableList<T> observableList) => observableList.Value;

        public IEnumerator<T> GetEnumerator() => Value.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}