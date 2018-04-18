using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Status", fileName = "ShipStatus")]
public class ShipStatusModel : ScriptableObject {
    public float maxHealth;
    public float maxEnergy;

    public uint maxAmmunition;
}