using UnityEngine;

public class SlidingModeControl {

    // The targeted point in world space
    public Vector3 targetPosition;
    public Vector3 targetAim;

    private Transform currentTransform;

    public Vector3 thrustFactors;
    public Vector3 torqueFactors;

    public Vector3 thrustDeltas;
    public Vector3 torqueDeltas;

    public SlidingModeControl (Transform currentTransform) {
        this.currentTransform = currentTransform;
    }

    public struct EngineInput {
        public static EngineInput zero = new EngineInput {
            thrust = Vector3.zero,
            torque = Vector3.zero
        };

        public Vector3 thrust;
        public Vector3 torque;
    }

    public EngineInput ComputeControl () {
        // Compute error
        EngineInput error = ComputeError();

        // Compute input from error
        return new EngineInput {
            thrust = Vector3.Scale(this.thrustFactors, error.thrust.Sign(this.thrustDeltas)),
            torque = Vector3.Scale(this.torqueFactors, error.torque.Sign(this.torqueDeltas))
        };
    }

    private EngineInput ComputeError () {
        // Position error in local space
        Vector3 positionError = this.currentTransform.InverseTransformPoint(this.targetPosition);

        // Attitude error in local space
        Quaternion attitudeError = Quaternion.Inverse(this.currentTransform.rotation)
                        * Quaternion.LookRotation(this.targetAim - this.currentTransform.position);

        // Correct attitude error from [0; 360] to [-180; 180]
        Vector3 correctedAttitudeError = attitudeError.eulerAngles;
        for (int i = 0; i < 3; i++)
            correctedAttitudeError[i] -= (correctedAttitudeError[i] > 180f) ? 360f : 0f;
        
        return new EngineInput {
            thrust = positionError,
            torque = correctedAttitudeError
        };
    }

}
