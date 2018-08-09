using UnityEngine;

public class ShipTargeter : AISubController {

    private Transform target {
        get { return this.shipSimulator.target; }
        set { this.shipSimulator.target = value; }
    }

    private ShipSimulator shipSimulator;

    private Transform currentTransform;

    public float desiredDistance;
    public float anticipationFactor;

    public ShipTargeter (Transform currentTransform, Transform target, float desiredDistance, float anticipationFactor) {
        this.shipSimulator = new ShipSimulator(target);

        this.currentTransform = currentTransform;

        this.desiredDistance = desiredDistance;
        this.anticipationFactor = anticipationFactor;
    }

    public override SlidingModeControl.Target ComputeTarget () {
        this.shipSimulator.anticipationTime = this.anticipationFactor * (this.target.position - this.currentTransform.position).magnitude;
        this.shipSimulator.UpdateSimulator();

        return new SlidingModeControl.Target {
            point = this.shipSimulator.simulatedPosition - (this.shipSimulator.simulatedPosition - this.currentTransform.position).normalized * desiredDistance,
            aim = this.shipSimulator.simulatedPosition
        };
    }
}
