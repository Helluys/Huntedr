using UnityEngine;

[System.Serializable]
public class ShipDynamics {
    private const float MAXIMUM_ANGULAR_VELOCITY = 30f;

    public Ship ship { get; }

    public float currentFluidDensity { get; set; }

    private Vector3 localVelocity { get { return transform.InverseTransformVector(rigidbody.velocity); } }
    private Vector3 localAngularVelocity { get { return transform.InverseTransformVector(rigidbody.angularVelocity); } }

    [SerializeField] private ShipDynamicsModel shipDynamicsInstance;
    private ShipEngine engine;
    private Rigidbody rigidbody;
    private Transform transform;

    public ShipDynamics (Ship holder) {
        ship = holder;
        engine = holder.engine;
        transform = holder.GetComponent<Transform>();

        // Instancing model allows per ship instance model variation
        shipDynamicsInstance = UnityEngine.Object.Instantiate(holder.model.dynamicsModel);

        rigidbody = holder.GetComponent<Rigidbody>();
        rigidbody.mass = shipDynamicsInstance.mass;
        rigidbody.inertiaTensor = shipDynamicsInstance.inertiaTensor;
        rigidbody.inertiaTensorRotation = Quaternion.identity;
        rigidbody.maxAngularVelocity = MAXIMUM_ANGULAR_VELOCITY;
        
        currentFluidDensity = 1f;
    }

    public void OnFixedUpdate () {
        // Apply forces
        this.rigidbody.AddRelativeForce(ComputeThrust(), ForceMode.Impulse);

        // Apply torques
        this.rigidbody.AddRelativeTorque(ComputeTorque(), ForceMode.Impulse);

        // Apply controlled dampening
        rigidbody.drag = engine.outputCushion;
        rigidbody.angularDrag = engine.outputStabilize;
    }

    private Vector3 ComputeThrust () {
        return engine.outputThrust + LinearFriction();
    }

    private Vector3 LinearFriction () {
        return currentFluidDensity * (shipDynamicsInstance.linearFrictionMatrix_linear * localVelocity
            + shipDynamicsInstance.linearFrictionMatrix_quadratic * Vector3.Scale(localVelocity, localVelocity));
    }

    private Vector3 ComputeTorque () {
        return engine.outputTorque + DynamicTorque(engine.outputTorque) + RotationalFriction();
    }

    private Vector3 DynamicTorque (Vector3 flatTorque) {
        Vector3 torque = Vector3.zero;
        for (int i = 0; i < 3; i++)
            torque[i] = shipDynamicsInstance.dynamicTorqueProfile[i].Evaluate(flatTorque[i] * currentFluidDensity * localVelocity.z);
        return torque;
    }

    private Vector3 RotationalFriction () {
        return currentFluidDensity * (shipDynamicsInstance.rotationalFrictionMatrix_linear * localAngularVelocity
            + shipDynamicsInstance.rotationalFrictionMatrix_quadratic * Vector3.Scale(localAngularVelocity, localAngularVelocity)
            + shipDynamicsInstance.linearToRotationMatrix * localVelocity);
    }

}
