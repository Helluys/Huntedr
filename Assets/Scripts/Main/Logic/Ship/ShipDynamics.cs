using System;
using UnityEngine;
using Utilities;

[Serializable]
public class ShipDynamics {
    private const float MAXIMUM_ANGULAR_VELOCITY = 30f;

    public Ship ship { get; }

    private Vector3 _inputThrust;
    public Vector3 inputThrust {
        get { return _inputThrust; }
        set { _inputThrust = MathUtils.ClampVector3(value, -1, 1); }
    }

    private Vector3 _inputTorque;
    public Vector3 inputTorque {
        get { return _inputTorque; }
        set { _inputTorque = MathUtils.ClampVector3(value, -1, 1); }
    }

    public float inputCushion { get; set; }
    public float inputStabilize { get; set; }

    public float currentFluidDensity { get; set; }

    private Vector3 localVelocity { get { return transform.InverseTransformVector(rigidbody.velocity); } }
    private Vector3 localAngularVelocity { get { return transform.InverseTransformVector(rigidbody.angularVelocity); } }

    [SerializeField] private ShipDynamicsModel shipDynamicsInstance;
    private Rigidbody rigidbody;
    private Transform transform;

    public ShipDynamics (Ship holder) {
        ship = holder;

        // Instancing model allows per ship instance model variation
        shipDynamicsInstance = UnityEngine.Object.Instantiate(holder.model.dynamicsModel);

        rigidbody = holder.GetComponent<Rigidbody>();
        rigidbody.mass = shipDynamicsInstance.mass;
        rigidbody.inertiaTensor = shipDynamicsInstance.inertiaTensor;
        rigidbody.inertiaTensorRotation = Quaternion.identity;
        rigidbody.maxAngularVelocity = MAXIMUM_ANGULAR_VELOCITY;

        transform = holder.transform;

        inputThrust = Vector3.zero;
        inputTorque = Vector3.zero;
        inputCushion = 0f;
        inputStabilize = 0f;
        currentFluidDensity = 1f;
    }

    public void OnFixedUpdate () {
        // Apply forces
        this.rigidbody.AddRelativeForce(ComputeThrust(), ForceMode.Impulse);

        // Apply torques
        this.rigidbody.AddRelativeTorque(ComputeTorque(), ForceMode.Impulse);

        // Apply controlled dampening
        rigidbody.drag = (Mathf.Atan(inputCushion) / Mathf.PI) * ship.model.dynamicsModel.cushionAbility;
        rigidbody.angularDrag = (Mathf.Atan(inputStabilize) / Mathf.PI) * ship.model.dynamicsModel.stabilizeAbility;
    }

    private Vector3 ComputeThrust () {
        return ThrustFromInput() + LinearFriction();
    }

    private Vector3 ThrustFromInput () {
        Vector3 thrust = Vector3.zero;
        for (int i = 0; i < 3; i++)
            thrust[i] = shipDynamicsInstance.flatThrustProfile[i].Evaluate(inputThrust[i]);
        return thrust;
    }

    private Vector3 LinearFriction () {
        return currentFluidDensity * (shipDynamicsInstance.linearFrictionMatrix_linear * localVelocity
            + shipDynamicsInstance.linearFrictionMatrix_quadratic * Vector3.Scale(localVelocity, localVelocity));
    }

    private Vector3 ComputeTorque () {
        return TorqueFromInput() + RotationalFriction();
    }

    private Vector3 TorqueFromInput () {
        Vector3 torque = Vector3.zero;
        for (int i = 0; i < 3; i++) {
            torque[i] = shipDynamicsInstance.flatTorqueProfile[i].Evaluate(inputTorque[i])
                      + shipDynamicsInstance.dynamicTorqueProfile[i].Evaluate(inputTorque[i] * currentFluidDensity * localVelocity.z);
        }
        return torque;
    }

    private Vector3 RotationalFriction () {
        return currentFluidDensity * (shipDynamicsInstance.rotationalFrictionMatrix_linear * localAngularVelocity
            + shipDynamicsInstance.rotationalFrictionMatrix_quadratic * Vector3.Scale(localAngularVelocity, localAngularVelocity)
            + shipDynamicsInstance.linearToRotationMatrix * localVelocity);
    }

}
