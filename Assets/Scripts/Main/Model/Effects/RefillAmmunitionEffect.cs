using UnityEngine;

[CreateAssetMenu(fileName = "RefillAmmunitionEffect", menuName = "Game data/Effects/Refill ammunition effect")]
public class RefillAmmunitionEffect : Effect {

    public uint refillAmount;

    protected override void Apply (Ship ship) {
        ship.status.RefillAmmunition(refillAmount);
    }
}
