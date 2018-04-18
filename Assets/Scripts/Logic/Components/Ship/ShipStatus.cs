using System;
using UnityEngine;

public class ShipStatus : MonoBehaviour {

    public event EventHandler<GameObject> OnDeath;

    [SerializeField] private ShipStatusModel shipStatusModel;
    private ShipStatusModel shipStatusInstance;

    private float health;
    private float energy;
    private uint ammunition;

    void Start () {
        shipStatusInstance = Instantiate(shipStatusModel);

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
            health = 0f;
            OnDeath(this, gameObject);
            killed = true;
        }

        return killed;
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
        }

        return allowed;
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
        }

        return allowed;
    }
}
