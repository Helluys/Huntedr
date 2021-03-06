﻿using System.Collections.Generic;

using UnityEngine;

public class MissileLauncher : WeaponSystem {

    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform shootBulletPoint;
    [SerializeField] private FloatStatistic cooldown = new FloatStatistic(1f);
    [SerializeField] private FloatStatistic shootVelocity = new FloatStatistic(1f);
    [SerializeField] private uint ammunitionPerUse = 1;
    [SerializeField] private float energyPerUse = 1f;
    [SerializeField] private ObjectDetector detector;

    private Ship ship;

    private float allowedShootTime;

    private List<Ship> detectedShips = new List<Ship>();
    private Transform lockedTarget;

    private void Start () {
        detector.OnObjectDetected += Detector_OnObjectDetected;
        detector.OnObjectLost += Detector_OnObjectLost;
    }

    private void Update () {
        SearchTarget();
    }

    private void SearchTarget () {
        Ship newTarget = null;

        foreach (Ship detectedShip in detectedShips) {
            if (detectedShip.team.faction.Equals(ship.team.faction) || detectedShip.isDestroyed)
                continue;
            else if (newTarget == null)
                    newTarget = detectedShip;
            else {
                float lockedTargetDistance = (newTarget.transform.position - transform.position).magnitude;
                float detectedShipDistance = (detectedShip.transform.position - transform.position).magnitude;

                if (detectedShipDistance < lockedTargetDistance)
                    newTarget = detectedShip;
            }
        }

        SetLockedTarget(newTarget);
    }

    private void SetLockedTarget (Ship detectedShip) {
        if ((detectedShip == null && lockedTarget == null) || (detectedShip != null && detectedShip.transform.Equals(lockedTarget)))
            return;

        if (lockedTarget != null)
            lockedTarget.GetComponent<Ship>().OnDestruction -= OnTargetDestruction;

        lockedTarget = detectedShip?.transform;

        if (lockedTarget != null)
            lockedTarget.GetComponent<Ship>().OnDestruction += OnTargetDestruction;
    }

    private void OnTargetDestruction (object sender, IDestructible target) {
        SearchTarget();
    }

    #region WeaponSystem interface

    public override void Initialize (Ship holder) {
        ship = holder;
    }

    public override void Shoot () {
        if (!CanShoot()) return;

        ship.status.TryUseAmmunition(ammunitionPerUse);
        ship.status.TryUseEnergy(energyPerUse);

        var missile = Instantiate(missilePrefab, shootBulletPoint.position, shootBulletPoint.rotation);
        missile.GetComponent<Rigidbody>().velocity = ship.GetComponent<Rigidbody>().GetPointVelocity(shootBulletPoint.position)
                                                        + shootVelocity * shootBulletPoint.forward;
        missile.GetComponent<Missile>().target = lockedTarget;

        allowedShootTime = Time.time + cooldown;
    }

    private bool CanShoot () {
        return Time.time > allowedShootTime
            && ship.status.GetAmmunition() >= ammunitionPerUse
            && ship.status.GetEnergy() > energyPerUse;
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

    private void Detector_OnObjectDetected (object sender, Collider collider) {
        Ship foundShip = collider.attachedRigidbody?.GetComponent<Ship>();
        if (foundShip != null && !detectedShips.Contains(foundShip))
            detectedShips.Add(foundShip);
    }

    public void Detector_OnObjectLost (object sender, Collider collider) {
        Ship foundShip = collider.attachedRigidbody?.GetComponent<Ship>();
        if (foundShip != null && detectedShips.Contains(foundShip))
            detectedShips.Remove(foundShip);
    }

    #endregion
}
