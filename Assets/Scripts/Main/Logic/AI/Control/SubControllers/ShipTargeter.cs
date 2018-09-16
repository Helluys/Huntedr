using UnityEngine;

public class ShipTargeter : AISubController {

    private Ship target {
        get { return this.targetSimulator.target; }
        set { this.targetSimulator.target = value; }
    }

    private readonly ShipSimulator targetSimulator;
    private readonly Ship ship;

    private readonly float firingThreshold = 1f;

    private readonly float desiredDistance;
    private readonly float anticipationFactor;

    public ShipTargeter (Ship ship, Ship target, float desiredDistance, float anticipationFactor) {
        this.targetSimulator = new ShipSimulator(target);

        this.ship = ship;

        this.desiredDistance = desiredDistance;
        this.anticipationFactor = anticipationFactor;
    }

    public override SlidingModeControl.Target ComputeTarget () {
        this.target.OnDestruction += TargetDestruction;

        this.targetSimulator.anticipationTime = this.anticipationFactor * (this.target.transform.position - this.ship.transform.position).magnitude;
        this.targetSimulator.UpdateSimulator();

        float targetingDistance = Vector3.ProjectOnPlane(this.targetSimulator.simulatedPosition - this.ship.transform.position, this.ship.transform.forward).magnitude;
        if (targetingDistance < this.firingThreshold)
            foreach (GameObject weaponSystem in this.ship.weaponSystems)
                weaponSystem.GetComponent<WeaponSystem>().Shoot();

        return new SlidingModeControl.Target {
            point = this.targetSimulator.simulatedPosition - (this.targetSimulator.simulatedPosition - this.ship.transform.position).normalized * this.desiredDistance,
            aim = this.targetSimulator.simulatedPosition
        };
    }

    private void TargetDestruction (object sender, IDestructible e) {
        ObjectiveCompleted();
    }
}
