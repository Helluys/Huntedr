using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSystem : MonoBehaviour {

    public abstract void Shoot ();
    
    private Dictionary<WeaponBuffEffect, FloatStatistic.Modifier> effectModifiers = new Dictionary<WeaponBuffEffect, FloatStatistic.Modifier>();

    public abstract void Initialize (Ship holder);

    public void Buff (WeaponBuffEffect effect) {
        FloatStatistic.Modifier modifier = new FloatStatistic.Modifier(FloatStatistic.Modifier.Type.Factor, effect.buffRatio);
        effectModifiers.Add(effect, modifier);
        ApplyModifier(modifier);
    }

    public void Debuff (WeaponBuffEffect effect) {
        if (!effectModifiers.ContainsKey(effect))
            throw new KeyNotFoundException("Inexistant weapon effect in Debuff");

        RemoveModifier(effectModifiers[effect]);

        effectModifiers.Remove(effect);
    }

    protected abstract void ApplyModifier (FloatStatistic.Modifier modifier);
    protected abstract void RemoveModifier (FloatStatistic.Modifier modifier);


}
