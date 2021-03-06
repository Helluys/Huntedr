﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RepairZone : MonoBehaviour {

    public int factionIndex;
    public float repairRate;
    public float energyRefillRate;
    public float ammunitionRefillRate;

    private Dictionary<Ship, Coroutine> repairCoroutines = new Dictionary<Ship, Coroutine>();

    private void OnTriggerEnter (Collider other) {
        Ship ship = other.attachedRigidbody.GetComponent<Ship>();
        
        if (ship != null && Faction.AreFriendly(Faction.FromIndex(factionIndex), ship.team.faction) && !repairCoroutines.ContainsKey(ship))
            repairCoroutines.Add(ship, StartCoroutine(Repair(ship)));
    }

    private void OnTriggerExit (Collider other) {
        Ship ship = other.attachedRigidbody.GetComponent<Ship>();

        if (ship != null && repairCoroutines.ContainsKey(ship)) {
            StopCoroutine(repairCoroutines[ship]);
            repairCoroutines.Remove(ship);
        }
    }

    private IEnumerator Repair (Ship shipToRepair) {
        uint ammunitionRefillAmount = 0;
        float ammunitionRefillLeftover = 0f;
        float ammunitionRefill = 0f;

        // Coroutine stopped by StopCoroutine
        for (; ; ) {
            // Repair health
            shipToRepair.status.Repair(repairRate * Time.deltaTime);

            // Refill energy
            shipToRepair.status.RefillEnergy(energyRefillRate * Time.deltaTime);

            // Refill ammunition : deal with decimal leftover of refill rate
            ammunitionRefill = ammunitionRefillRate * Time.deltaTime + ammunitionRefillLeftover;
            ammunitionRefillAmount = (uint) Mathf.FloorToInt(ammunitionRefill);
            ammunitionRefillLeftover = ammunitionRefill - ammunitionRefillAmount;
            shipToRepair.status.RefillAmmunition(ammunitionRefillAmount);

            yield return null;
        }
    }
}
