using UnityEngine;

[CreateAssetMenu(fileName = "RepairEffect", menuName = "Game data/Effects/Repair effect")]
public class RepairEffect : Effect {
    public float repairRate;

    protected override void Apply (Ship ship) {
        ship.status.Repair(repairRate * Time.deltaTime);
    }
}
