using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public WinCondition winConditions;

    public GameConfiguration gameConfiguration;
    public List<Ship> playerList = new List<Ship>();

    [SerializeField] GameObject shipPrefab;
    [SerializeField] ShipController playerController;

    [SerializeField] private List<SpawningZone> spawningZones = new List<SpawningZone>();

    private void SetUpSingleton () {
        if (instance != null)
            Debug.LogError("Only one instance of gameManager is allowed");
        else
            instance = this;
    }

    private void Start() {
        SetUpSingleton();
        
        CreateShips();

        winConditions.Setup();
    }

    public static bool AreFriendlyFactions (Faction faction1, Faction faction2) {
        return faction1 == faction2;
    }

    public static SpawningZone GetSpawningZone(Faction faction) {
        return instance.spawningZones[faction.index];
    }

    private void CreateShips () {
        foreach(TeamConfiguration teamConfiguration in gameConfiguration.teams) {
            foreach (ShipConfiguration shipConfiguration in teamConfiguration.ships) {
                Ship ship = Instantiate(shipPrefab).GetComponent<Ship>();
                ship.faction = teamConfiguration.faction;

                ship.name = shipConfiguration.name;
                ship.model = shipConfiguration.shipModel;
                ship.controller = shipConfiguration.shipController;

                if (shipConfiguration.shipController.Equals(playerController))
                    playerList.Add(ship);

                ship.ResetModels();
                ship.Respawn();
            }
        }
    }
}
