using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : WeaponSystem {

    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform shootBulletPoint;
    [SerializeField] private FloatStatistic cooldown = new FloatStatistic(1);

    private Ship ship;
    
    private float allowedShootTime;

    private List<Ship> detectedShips = new List<Ship>();
    private Transform lockedTarget;

    private void Update () {
        lockedTarget = null;
        foreach (Ship detectedShip in detectedShips) {
            if (detectedShip.faction.Equals(ship.faction))
                continue;
            else if (lockedTarget == null)
                lockedTarget = detectedShip.transform;
            else {
                float lockedTargetDistance = (lockedTarget.position - transform.position).magnitude;
                float detectedShipDistance = (detectedShip.transform.position - transform.position).magnitude;

                if (detectedShipDistance < lockedTargetDistance)
                    lockedTarget = detectedShip.transform;
            }

        }
    }

    #region WeaponSystem interface

    public override void Initialize (Ship holder) {
        ship = holder;
    }

    public override void Shoot () {
        if (!CanShoot()) return;

        var missile = Instantiate(missilePrefab, shootBulletPoint.position, shootBulletPoint.rotation);
        missile.GetComponent<Rigidbody>().velocity = ship.GetComponent<Rigidbody>().GetPointVelocity(shootBulletPoint.position);
        missile.GetComponent<Missile>().target = lockedTarget;

        allowedShootTime = Time.time + cooldown;
    }

    private bool CanShoot () {
        return Time.time > allowedShootTime && ship.status.TryUseAmmunition(1);
    }

    protected override void ApplyModifier (FloatStatistic.Modifier modifier) {
        // TODO : customizable modifiers (invert effect)
        cooldown.AddModifier(modifier);
    }

    protected override void RemoveModifier (FloatStatistic.Modifier modifier) {
        cooldown.RemoveModifier(modifier);
    }

    #endregion

    #region ship detection

    private void OnTriggerEnter (Collider other) {
        Ship foundShip = other.attachedRigidbody.GetComponent<Ship>();
        if (foundShip != null && !detectedShips.Contains(foundShip))
            detectedShips.Add(foundShip);
    }

    private void OnTriggerExit (Collider other) {
        Ship foundShip = other.attachedRigidbody.GetComponent<Ship>();
        if (foundShip != null && detectedShips.Contains(foundShip))
            detectedShips.Remove(foundShip);
    }

    #endregion
}
