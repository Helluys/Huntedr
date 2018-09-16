using UnityEngine;

public class ObjectTargeter : AISubController {

    private readonly Ship ship;
    private readonly IDestructible target;

    private readonly float firingThreshold = 1f;

    private readonly float desiredDistance;

    public ObjectTargeter (Ship ship, IDestructible target, float desiredDistance) {
        this.ship = ship;
        this.target = target;

        this.desiredDistance = desiredDistance;
    }

    public override SlidingModeControl.Target ComputeTarget () {
        this.target.OnDestruction += TargetDestruction;

        float targetingDistance = Vector3.ProjectOnPlane(this.target.gameObject.transform.position - this.ship.transform.position, this.ship.transform.forward).magnitude;
        if (targetingDistance < this.firingThreshold)
            foreach (GameObject weaponSystem in this.ship.weaponSystems)
                weaponSystem.GetComponent<WeaponSystem>().Shoot();

        return new SlidingModeControl.Target {
            point = this.target.gameObject.transform.position - (this.target.gameObject.transform.position - this.ship.transform.position).normalized * this.desiredDistance,
            aim = this.target.gameObject.transform.position
        };
    }

    private void TargetDestruction (object sender, IDestructible e) {
        ObjectiveCompleted();
    }
}
