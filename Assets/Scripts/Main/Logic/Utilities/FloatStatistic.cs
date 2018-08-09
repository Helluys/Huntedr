using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class FloatStatistic {

    public event EventHandler<float> OnValueChanged;

    [Serializable]
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

    private ISet<Modifier> modifiers = new HashSet<Modifier>();
    
    [SerializeField] private float baseValue;
    public float value {
        get {
            float val = baseValue;
            foreach (Modifier modifier in modifiers)
                val = modifier.Apply(val);
            return val;
        }
    }

    public FloatStatistic (float baseValue) {
        this.baseValue = baseValue;
    }

    public void AddModifier (Modifier modifier) {
        modifiers.Add(modifier);
        OnValueChanged?.Invoke(this, value);
    }

    public void RemoveModifier (Modifier modifier) {
        modifiers.Remove(modifier);
        OnValueChanged?.Invoke(this, value);
    }

    public static implicit operator float(FloatStatistic s) {
        return s.value;
    }
}