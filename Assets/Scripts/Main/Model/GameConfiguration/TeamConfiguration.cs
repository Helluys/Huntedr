using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Team configuration", menuName = "Game data/Game configuration/Team")]
public class TeamConfiguration : ScriptableObject {

    new public string name;
    public Faction faction;
    public List<ShipConfiguration> ships = new List<ShipConfiguration>();

}
