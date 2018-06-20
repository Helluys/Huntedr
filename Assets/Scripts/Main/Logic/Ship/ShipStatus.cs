using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShipStatus {

    public event EventHandler<float> OnHealthChanged;
    public event EventHandler<float> OnEnergyChanged;
    public event EventHandler<float> OnAmmunitionChanged;
    public event EventHandler<Ship> OnDeath;

    public Ship ship { get; }
    [SerializeField] private float health;
    [SerializeField] private float energy;
    [SerializeField] private uint ammunition;

    // The ship status of this instance, may be modified by effects
    private ShipStatusModel shipStatusInstance;

    private List<Effect> effects = new List<Effect>();

    public ShipStatus (Ship holder) {
        ship = holder;
        shipStatusInstance = UnityEngine.Object.Instantiate(ship.model.statusModel);

        ResetStatus();
    }

    #region status

    public void ResetStatus () {
        health = shipStatusInstance.maxHealth;
        energy = shipStatusInstance.maxEnergy;
        ammunition = shipStatusInstance.maxAmmunition;
    }

    /// <summary>
    /// Apply damage to the ship status.
    /// </summary>
    /// <param name="damage">The amount of damage to apply</param>
    /// <returns>True if this damage killed the ship</returns>
    public bool Damage (float damage) {
        bool killed = false;

        health -= damage;

        if (health <= 0f) {
            Destroy();
            killed = true;
        }

        OnHealthChanged?.Invoke(this, -damage);
        return killed;
    }

    public void Destroy () {
        health = 0f;
        
        OnDeath?.Invoke(this, ship);
    }

    /// <summary>
    /// Repair the ship.
    /// </summary>
    /// <param name="amount">The amount of health to repair</param>
    /// <returns>The amount of health actually repaired</returns>
    public float Repair (float amount) {
        float repairedAmount = amount;
        health += amount;

        if (health >= shipStatusInstance.maxHealth) {
            repairedAmount = amount - (health - shipStatusInstance.maxHealth);
            health = shipStatusInstance.maxHealth;
        }

        OnHealthChanged?.Invoke(this, amount);
        return repairedAmount;
    }

    /// <summary>
    /// Try to use the given energy amount. No energy is used if it was not fully available.
    /// </summary>
    /// <param name="amount">The amount of energy to use</param>
    /// <returns>True if the energy was available and used</returns>
    public bool TryUseEnergy (float amount) {
        bool allowed = false;

        if (energy >= amount) {
            energy -= amount;
            allowed = true;

            OnEnergyChanged?.Invoke(this, -amount);
        }

        return allowed;
    }

    public float RefillEnergy (float amount) {
        float refillAmount = amount;
        energy += amount;

        if (energy >= shipStatusInstance.maxEnergy) {
            refillAmount = amount - (energy - shipStatusInstance.maxEnergy);
            energy = shipStatusInstance.maxEnergy;
        }

        OnEnergyChanged?.Invoke(this, amount);
        return refillAmount;
    }

    /// <summary>
    /// Try to use the given ammunition amount. No ammunition is used if it was not fully available.
    /// </summary>
    /// <param name="amount">The amount of ammunition to use</param>
    /// <returns>True if the ammunition was available and used</returns>
    public bool TryUseAmmunition (uint amount) {
        bool allowed = false;

        if (ammunition >= amount) {
            ammunition -= amount;
            allowed = true;

            OnAmmunitionChanged?.Invoke(this, -amount);
        }

        return allowed;
    }

    public uint RefillAmmunition (uint amount) {
        uint refillAmount = amount;
        ammunition += amount;

        if (ammunition >= shipStatusInstance.maxAmmunition) {
            refillAmount = amount - (ammunition - shipStatusInstance.maxAmmunition);
            ammunition = shipStatusInstance.maxAmmunition;
        }

        OnAmmunitionChanged?.Invoke(this, amount);
        return refillAmount;
    }

    #endregion

    #region getters

    public float GetHealth () { return health; }
    public float GetEnergy () { return energy; }
    public uint GetAmmunition () { return ammunition; }

    #endregion

    #region effects

    public void AddEffect(Effect effect) {
        if (!effects.Contains(effect)) {
            effects.Add(effect);
            effect.Activate(ship);
            effect.OnDeactivation += RemoveEffect;
        }
    }

    public void RemoveEffect (Effect effect) {
        if (effect.active)
            effect.Deactivate();
    }

    private void RemoveEffect(object source, Effect effect) {
        effects.Remove(effect);
    }

    #endregion
}
