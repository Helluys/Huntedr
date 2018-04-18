using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipDynamics : MonoBehaviour {

    private const float MAXIMUM_ANGULAR_VELOCITY = 30f;

    public Vector3 inputThrust { get; set; }
    public Vector3 inputTorque { get; set; }
    public float inputCushion { get; set; }
    public float inputStabilize { get; set; }

    public float currentFluidDensity { get; set; }

    private Vector3 localVelocity { get { return transform.InverseTransformVector(rigidbody.velocity); } }
    private Vector3 localAngularVelocity { get { return transform.InverseTransformVector(rigidbody.angularVelocity); } }

    [SerializeField] private ShipDynamicsModel shipDynamicsModel;
    private ShipDynamicsModel shipDynamicsInstance;
    private new Rigidbody rigidbody;

    void Start () {
        // Instancing model allows per ship instance model variation
        shipDynamicsInstance = Instantiate(shipDynamicsModel);

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = shipDynamicsInstance.mass;
        rigidbody.inertiaTensor = shipDynamicsInstance.inertiaTensor;
        rigidbody.inertiaTensorRotation = Quaternion.identity;
        rigidbody.maxAngularVelocity = MAXIMUM_ANGULAR_VELOCITY;

        inputThrust = Vector3.zero;
        inputTorque = Vector3.zero;
        inputCushion = 0f;
        inputStabilize = 0f;
        currentFluidDensity = 1f;
    }

    void FixedUpdate () {
        // Apply forces
        Vector3 thrust = ThrustFromInput();
        Vector3 friction = currentFluidDensity * (shipDynamicsInstance.linearFrictionMatrix * localVelocity);
        rigidbody.AddRelativeForce(thrust + friction, ForceMode.Impulse);

        // Apply torques
        Vector3 torque = TorqueFromInput();
        Vector3 rotFromSpeed = currentFluidDensity * (shipDynamicsInstance.linearToRotationMatrix * localVelocity);
        Vector3 rotFriction = currentFluidDensity * (shipDynamicsInstance.rotationalFrictionMatrix * localAngularVelocity);
        rigidbody.AddRelativeTorque(torque + rotFromSpeed + rotFriction, ForceMode.Impulse);

        // Apply controlled dampening
        rigidbody.drag = (Mathf.Atan(inputCushion) / Mathf.PI) * shipDynamicsModel.cushionAbility;
        rigidbody.angularDrag = (Mathf.Atan(inputStabilize) / Mathf.PI) * shipDynamicsModel.stabilizeAbility;
    }

    private Vector3 ThrustFromInput () {
        return new Vector3(
            (Mathf.Atan(Mathf.Abs(inputThrust.x)) / Mathf.PI) * (inputThrust.x > 0f ? shipDynamicsInstance.maxThrust.x : shipDynamicsInstance.minThrust.x),
            (Mathf.Atan(Mathf.Abs(inputThrust.y)) / Mathf.PI) * (inputThrust.y > 0f ? shipDynamicsInstance.maxThrust.y : shipDynamicsInstance.minThrust.y),
            (Mathf.Atan(Mathf.Abs(inputThrust.z)) / Mathf.PI) * (inputThrust.z > 0f ? shipDynamicsInstance.maxThrust.z : shipDynamicsInstance.minThrust.z));
    }

    private Vector3 TorqueFromInput () {
        float normalizedSpeed = localVelocity.z / shipDynamicsInstance.maxSpeed;
        Debug.Log(normalizedSpeed);
        return new Vector3(
            (Mathf.Atan(inputTorque.x) / Mathf.PI) * (shipDynamicsInstance.flatTorque.x + currentFluidDensity * shipDynamicsInstance.torqueProfile[0].Evaluate(normalizedSpeed)),
            (Mathf.Atan(inputTorque.y) / Mathf.PI) * (shipDynamicsInstance.flatTorque.y + currentFluidDensity * shipDynamicsInstance.torqueProfile[1].Evaluate(normalizedSpeed)),
            (Mathf.Atan(inputTorque.z) / Mathf.PI) * (shipDynamicsInstance.flatTorque.z + currentFluidDensity * shipDynamicsInstance.torqueProfile[2].Evaluate(normalizedSpeed)));
    }

}
