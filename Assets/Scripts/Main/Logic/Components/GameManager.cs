using System;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    private Dictionary<Faction, SpawningZone> spawningZones = new Dictionary<Faction, SpawningZone>();

    void Start() {
        if (instance != null) {
            Debug.LogError("Only one instance of gameManager is allowed");
            Destroy(this);
        } else {
            instance = this;
        }
    }

    public static void AddSpawningZone (SpawningZone spawningZone) {
        instance.spawningZones.Add(spawningZone.faction, spawningZone);
    }

    public static bool AreFriendlyFactions (Faction faction1, Faction faction2) {
        return faction1 == faction2;
    }

    public static SpawningZone GetSpawningZone(Faction faction) {
        return instance.spawningZones[faction];
    }
}
