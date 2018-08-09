using UnityEngine;

public class SlidingModeControl {

    public struct Target {
        public Vector3 point;
        public Vector3 aim;
    }

    public Target target;
    
    private Transform currentTransform;

    public Vector3 thrustFactors;
    public Vector3 torqueFactors;

    public Vector3 thrustDeltas;
    public Vector3 torqueDeltas;
    
    public SlidingModeControl (Transform currentTransform) {
        this.currentTransform = currentTransform;
    }

    public ShipEngine.Input ComputeControl () {
        // Compute error
        ShipEngine.Input error = ComputeError();

        // Compute input from error
        return new ShipEngine.Input {
            thrust = Vector3.Scale(this.thrustFactors, error.thrust.Sign(this.thrustDeltas)),
            torque = Vector3.Scale(this.torqueFactors, error.torque.Sign(this.torqueDeltas))
        };
    }

    private ShipEngine.Input ComputeError () {
        // Position error in local space
        Vector3 positionError = this.currentTransform.InverseTransformPoint(this.target.point);

        // Attitude error in local space
        Quaternion attitudeError = Quaternion.Inverse(this.currentTransform.rotation)
                        * Quaternion.LookRotation(this.target.aim - this.currentTransform.position);

        // Correct attitude error from [0; 360] to [-180; 180]
        Vector3 correctedAttitudeError = attitudeError.eulerAngles;
        for (int i = 0; i < 3; i++)
            correctedAttitudeError[i] -= (correctedAttitudeError[i] > 180f) ? 360f : 0f;

        return new ShipEngine.Input {
            thrust = positionError,
            torque = correctedAttitudeError
        };
    }

}
