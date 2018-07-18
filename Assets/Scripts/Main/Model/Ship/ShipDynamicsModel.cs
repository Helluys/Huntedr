using UnityEngine;
using Utilities;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Dynamics", fileName = "ShipDynamics")]
public class ShipDynamicsModel : ScriptableObject {

    public float mass = 1f;
    public Vector3 inertiaTensor = 5f * Vector3.one;

    public Vector3 centerOfMass = Vector3.zero;

    // The flat thrust from the normalized input for each axis
    public AnimationCurve[] flatThrustProfile = new AnimationCurve[3] {
        AnimationCurve.Linear(-1, -2, 1, 2),
        AnimationCurve.Linear(-1, -2, 1, 2),
        AnimationCurve.Linear(-1, -10, 1, 10)
    };

    // The flat torque from the normalized input for each axis
    public AnimationCurve[] flatTorqueProfile = new AnimationCurve[3] {
                AnimationCurve.Linear(-1, -0.1f, 1, 0.1f),
                AnimationCurve.Linear(-1, -0.1f, 1, 0.1f),
                AnimationCurve.Linear(-1, -0.1f, 1, 0.1f)
    };

    // The dynamic torque from the normalized input for each axis, multiplied by fluid density to create torque
    public AnimationCurve[] dynamicTorqueProfile = new AnimationCurve[3] {
                AnimationCurve.Linear(-1, -0.1f, 1, 0.1f),
                AnimationCurve.Linear(-1, -0.1f, 1, 0.1f),
                AnimationCurve.Linear(-1, -0.1f, 1, 0.1f)
    };

    // The linear factors for linear friction, multiplied by fluid density and velocity to create dampening
    public Matrix4x4 linearFrictionMatrix_linear = MathUtils.ScaleMatrix(Matrix4x4.identity, -0.2f);

    // The quadratic factors for linear friction, multiplied by fluid density and velocity squared to create dampening
    public Matrix4x4 linearFrictionMatrix_quadratic = Matrix4x4.zero;

    // The linear factors for rotational friction, multiplied by fluid density and rotational velocity to create rotational dampening
    public Matrix4x4 rotationalFrictionMatrix_linear = MathUtils.ScaleMatrix(Matrix4x4.identity, -0.2f);

    // The quadratic factors for rotational friction, multiplied by fluid density and rotational velocity squared to create rotational dampening
    public Matrix4x4 rotationalFrictionMatrix_quadratic = Matrix4x4.zero;

    // The factors that induce torque due to linear friction, multiplied by fluid density and velocitiy to create torque
    public Matrix4x4 linearToRotationMatrix = Matrix4x4.zero;

    public float maxSpeed = 10f;
    
}
