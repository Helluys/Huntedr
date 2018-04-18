using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship", fileName = "Ship")]
public class ShipModel : ScriptableObject {
    public ShipDynamicsModel dynamicsModel;
    public ShipStatusModel statusModel;
}