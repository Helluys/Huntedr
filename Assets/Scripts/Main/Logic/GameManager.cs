using System;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; private set; }

    public static Canvas uiCanvas { get; private set; }

    public WinCondition winConditions;

    public GameConfiguration gameConfiguration;

    public IReadOnlyList<Ship> playerList { get { return this._playerList.AsReadOnly(); } }
    public IReadOnlyList<Ship> shipList { get { return this._shipList.AsReadOnly(); } }
    public IReadOnlyList<Team> teamList { get { return this._teamList; } }

    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private ShipControllerModel playerController;

    [SerializeField] private List<SpawningZone> spawningZones = new List<SpawningZone>();
    [SerializeField] private float killDistance;

    [SerializeField] private GameObject pathNodeHolder;

    private readonly List<Team> _teamList = new List<Team>();
    private readonly List<Ship> _playerList = new List<Ship>();
    private readonly List<Ship> _shipList = new List<Ship>();

    private void SetUpSingleton () {
        if (instance != null)
            Debug.LogError("Only one instance of gameManager is allowed");
        else
            instance = this;
    }

    private void Start () {
        SetUpSingleton();

        uiCanvas = FindObjectOfType<Canvas>();

        CreateShips();

        this.winConditions.Setup();

        foreach(Team team in _teamList) {
            team.ai.Start();
        }
    }

    private void Update () {
        foreach (Ship ship in this._shipList)
            if (ship.transform.position.magnitude > this.killDistance)
                ship.Destroy();
    }

    public static SpawningZone GetSpawningZone (Faction faction) {
        return instance.spawningZones[faction.index];
    }

    public static GameObject GetPathNodesHolder () {
        return instance.pathNodeHolder;
    }

    private void CreateShips () {
        foreach (TeamConfiguration teamConfiguration in this.gameConfiguration.teams) {
            List<Ship> teamShips = new List<Ship>();
            Team team = new Team(teamConfiguration.name, teamConfiguration.faction, teamShips, teamConfiguration.aiPersonality);

            foreach (ShipConfiguration shipConfiguration in teamConfiguration.ships) {
                Ship ship = Instantiate(this.shipPrefab).GetComponent<Ship>();
                ship.team = team;

                ship.name = shipConfiguration.name;
                ship.model = shipConfiguration.shipModel;

                this._shipList.Add(ship);
                if (shipConfiguration.shipControllerModel.Equals(this.playerController))
                    this._playerList.Add(ship);
                teamShips.Add(ship);

                ship.ResetModels();
                ship.SetControllerModel(shipConfiguration.shipControllerModel);
                ship.Respawn();
            }

            this._teamList.Add(team);
        }
    }

    public List<Ship> FilterShips (Predicate<Ship> filter) {
        return this._shipList.FindAll(filter);
    }
}
