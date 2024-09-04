using System;

namespace Lando.Plugins.Observables
{
    [Serializable]
    public class ObservableInt : ObservableVariable<int>
    {
        public ObservableInt(int value = 0) 
            => Set(value);
        
        public sealed override void Set(int newValue)
        {
            _value = newValue;
            Notify();
        }

        private bool Equals(ObservableInt other) => Value == other.Value;
        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.GetType() == GetType() && Equals((ObservableInt)other);
        }
        public override int GetHashCode() => Value;

        public static implicit operator int(ObservableInt observableInt) => observableInt.Value;

        public static ObservableInt operator +(ObservableInt observableInt, int value)
        {
            observableInt.Value += value;
            return observableInt;
        }

        public static ObservableInt operator -(ObservableInt observableInt, int value)
        {
            observableInt.Value -= value;
            return observableInt;
        }

        public static ObservableInt operator ++(ObservableInt observableInt)
        {
            observableInt.Value++;
            return observableInt;
        }

        public static ObservableInt operator --(ObservableInt observableInt)
        {
            observableInt.Value--;
            return observableInt;
        }

        public static bool operator ==(ObservableInt a, int value) => a?.Value == value;
        public static bool operator !=(ObservableInt a, int value) => a?.Value != value;
        public static bool operator >(ObservableInt a, int value) => a.Value > value;
        public static bool operator <(ObservableInt a, int value) => a.Value < value;
        public static bool operator >=(ObservableInt a, int value) => a.Value >= value;
        public static bool operator <=(ObservableInt a, int value) => a.Value <= value;

        public static bool operator ==(int value, ObservableInt b) => value == b?.Value;
        public static bool operator !=(int value, ObservableInt b) => value != b?.Value;
        public static bool operator >(int value, ObservableInt b) => value > b.Value;
        public static bool operator <(int value, ObservableInt b) => value < b.Value;
        public static bool operator >=(int value, ObservableInt b) => value >= b.Value;
        public static bool operator <=(int value, ObservableInt b) => value <= b.Value;

        public static ObservableInt operator +(ObservableInt observableInt, ObservableInt value)
        {
            observableInt.Value += value.Value;
            return observableInt;
        }

        public static ObservableInt operator -(ObservableInt observableInt, ObservableInt value)
        {
            observableInt.Value -= value.Value;
            return observableInt;
        }

        public static bool operator ==(ObservableInt a, ObservableInt b) => a?.Value == b?.Value;
        public static bool operator !=(ObservableInt a, ObservableInt b) => a?.Value != b?.Value;
        public static bool operator >(ObservableInt a, ObservableInt b) => a.Value > b.Value;
        public static bool operator <(ObservableInt a, ObservableInt b) => a.Value < b.Value;
        public static bool operator >=(ObservableInt a, ObservableInt b) => a.Value >= b.Value;
        public static bool operator <=(ObservableInt a, ObservableInt b) => a.Value <= b.Value;

        public void UpdateValue(int newValue)
        {
            Value = newValue;
        }
    }
}