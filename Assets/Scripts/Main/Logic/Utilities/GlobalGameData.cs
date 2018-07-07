using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "GlobalGameData", menuName = "Game data/Global game data")]
public class GlobalGameData : ScriptableObject {

    public const string MAP_FOLDER = "Assets/GameData/Maps/";

    public List<MapData> maps = new List<MapData>();
    public List<ShipModel> shipModels = new List<ShipModel>();
    public List<ShipController> shipControllers = new List<ShipController>();
    public List<Faction> factions = new List<Faction>();

    [SerializeField] private TeamConfiguration emptyTeamConfiguration;
    [SerializeField] private ShipConfiguration emptyShipConfiguration;

    [Serializable]
    public class MapData {
        public string prettyName;
        public string sceneName;
        public int teamCount;
    }

    public List<TeamConfiguration> GetDefaultTeamConfigurationRange (int firstIndex, int lastIndex) {
        List<TeamConfiguration> teamList = new List<TeamConfiguration>(lastIndex - firstIndex);

        for (int index = firstIndex; index <= lastIndex; index++)
            teamList.Add(GetDefaultTeamConfiguration(index));

        return teamList;
    }

    public TeamConfiguration GetDefaultTeamConfiguration (int index) {
        TeamConfiguration teamConfiguration = Instantiate(emptyTeamConfiguration);
        teamConfiguration.name = "Team " + (index+1);
        teamConfiguration.faction = factions[index];
        teamConfiguration.ships = new List<ShipConfiguration>();

        return teamConfiguration;
    }

    public List<ShipConfiguration> GetDefaultShipConfigurationRange (int firstIndex, int lastIndex) {
        List<ShipConfiguration> shipList = new List<ShipConfiguration>(lastIndex - firstIndex);

        for (int index = firstIndex; index <= lastIndex; index++)
            shipList.Add(GetDefaultShipConfiguration(index));

        return shipList;
    }

    public ShipConfiguration GetDefaultShipConfiguration (int index) {
        ShipConfiguration shipConfiguration = Instantiate(emptyShipConfiguration);
        shipConfiguration.name = "Ship " + (index+1);
        shipConfiguration.shipModel = shipModels[0];
        shipConfiguration.shipController = shipControllers[0];

        return shipConfiguration;
    }
}
