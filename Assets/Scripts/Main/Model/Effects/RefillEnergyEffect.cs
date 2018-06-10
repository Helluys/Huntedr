using UnityEngine;

[CreateAssetMenu(fileName = "RefillEnergyEffect", menuName = "Game data/Effects/Refill energy effect")]
public class RefillEnergyEffect : Effect {
    public float refillRate;

    protected override void Apply (Ship ship) {
        ship.status.RefillEnergy(refillRate * Time.deltaTime);
    }
}
