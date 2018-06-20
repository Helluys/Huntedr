using System;
using System.Collections.Generic;

[Serializable]
public class FloatStatistic {

    public class Modifier {
        public enum Type {
            Flat, Factor
        }

        private Type type;
        private float value;

        public Modifier (Type type, float value) {
            this.type = type;
            this.value = value;
        }

        public float Apply (float value) {
            switch (type) {
                case Type.Flat:
                    return this.value + value;
                case Type.Factor:
                    return this.value * value;
            }
            throw new InvalidOperationException("Unhandled enum value for Statistic.Modifier.Type");
        }
    }

    private float baseValue;
    public float value {
        get {
            float val = baseValue;
            foreach (Modifier modifier in modifiers)
                val = modifier.Apply(val);
            return val;
        }
    }

    private ISet<Modifier> modifiers = new HashSet<Modifier>();

    public FloatStatistic (float baseValue) {
        this.baseValue = baseValue;
    }

    public void AddModifier (Modifier modifier) {
        modifiers.Add(modifier);
    }

    public void RemoveModifier (Modifier modifier) {
        modifiers.Remove(modifier);
    }

    public static implicit operator float(FloatStatistic s) {
        return s.value;
    }
}