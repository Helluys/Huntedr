using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class ShipAbilities {

    [SerializeField] List<Ability> abilities = new List<Ability>();

    private Ship ship;
    private List<Ability.Instance> abilityInstances = new List<Ability.Instance>();

    // Use this for initialization
    public void OnStart (Ship holder) {
        ship = holder;
        foreach (Ability ability in abilities)
            abilityInstances.Add(ability.CreateInstance(ship));
    }

    public void UseAbility(int index) {
        if (index < abilityInstances.Count)
            abilityInstances[index].Use();
    }

    public void ReleaseAbility (int index) {
        if (index < abilityInstances.Count)
            abilityInstances[index].Release();
    }

    public void ReleaseAll () {
        foreach (Ability.Instance ability in abilityInstances)
            ability.Release();
    }
}
