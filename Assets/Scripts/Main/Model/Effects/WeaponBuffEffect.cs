using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBuffEffect", menuName = "Game data/Effects/Weapon buff effect")]
public class WeaponBuffEffect : Effect {

    public float buffRatio;

    protected override void Apply (Ship ship) {
        foreach(WeaponSystem.IInstance weaponSystem in ship.weaponSystems) {
            // Instantiate buff
            WeaponBuffEffect spawnedEffect = Instantiate(this);

            // Initiate buff
            weaponSystem.Buff(spawnedEffect);

            // Attach debuff event
            EventHandler<Effect> debuffHandler = (object source, Effect effect) => weaponSystem.Debuff(spawnedEffect);
            OnDeactivation += debuffHandler;
        }
    }
}
