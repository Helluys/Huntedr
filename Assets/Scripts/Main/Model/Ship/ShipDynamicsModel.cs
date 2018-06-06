using UnityEngine;
using Utilities;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Dynamics", fileName = "ShipDynamics")]
public class ShipDynamicsModel : ScriptableObject {

    public float mass = 1f;
    public Vector3 inertiaTensor = 5f * Vector3.one;

    public Vector3 centerOfMass = Vector3.zero;

    public Vector3 minThrust = -Vector3.one;
    public Vector3 maxThrust = Vector3.one;
    public Vector3 flatTorque = Vector3.one;
    
    public AnimationCurve[] torqueProfile = new AnimationCurve[3] {
        AnimationCurve.Linear(-1, -1, 1, 1),
        AnimationCurve.Linear(-1, -1, 1, 1),
        AnimationCurve.Linear(-1, -1, 1, 1)
    };

    public Matrix4x4 linearFrictionMatrix = MathUtils.ScaleMatrix (Matrix4x4.identity, -0.2f);
    public Matrix4x4 rotationalFrictionMatrix = MathUtils.ScaleMatrix(Matrix4x4.identity, -0.2f);
    public Matrix4x4 linearToRotationMatrix = Matrix4x4.zero;

    public float cushionAbility;
    public float stabilizeAbility;

    public float maxSpeed = 10f;
}
