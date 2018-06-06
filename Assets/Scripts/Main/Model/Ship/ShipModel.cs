using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Model", fileName = "Ship")]
public class ShipModel : ScriptableObject {
    public ShipDynamicsModel dynamicsModel;
    public ShipStatusModel statusModel;
    public List<WeaponSystem> weaponSystems;
}