using UnityEngine;

[CreateAssetMenu(fileName = "Ship configuration", menuName = "Game data/Game configuration/Ship")]
public class ShipConfiguration : ScriptableObject {

    new public string name;
    public ShipModel shipModel;
    public ShipController shipController;

}