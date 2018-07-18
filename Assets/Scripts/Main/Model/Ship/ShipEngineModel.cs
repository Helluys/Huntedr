using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Engines", fileName = "ShipEngine")]
public class ShipEngineModel : ScriptableObject {

    public float energyProduction = 3f;

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

    // The factor of drag from cushion
    public float cushionAbility = 1f;

    // The factor of angular drag from stabilization
    public float stabilizeAbility = 1f;

    // The factors of energy consumption from thrust output
    public Vector3 thrustConsumption = Vector3.one;

    // The factors of energy consumption from torque output
    public Vector3 torqueConsumption = Vector3.one;

    // The factors of energy consumption from cushion output
    public float cushionConsumption = 1f;

    // The factors of energy consumption from stabilize output
    public float stabilizeConsumption = 1f;
}
