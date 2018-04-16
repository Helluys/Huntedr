using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipDynamics : MonoBehaviour {
    
    public Vector3 inputThrust { get; set; }
    public Vector3 inputTorque { get; set; }

    public float currentFluidDensity { get; set; }

    private Vector3 localVelocity { get { return transform.InverseTransformVector(rigidbody.velocity); } }
    private Vector3 localAngularVelocity { get { return transform.InverseTransformVector(rigidbody.angularVelocity); } }

    [SerializeField] private ShipModel shipModel;
    private ShipModel shipModelInstance;
    private new Rigidbody rigidbody;
    
    void Start() {
        shipModelInstance = Instantiate(shipModel);

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = shipModelInstance.mass;
        rigidbody.inertiaTensor = shipModelInstance.inertiaTensor;
        rigidbody.inertiaTensorRotation = Quaternion.identity;
        rigidbody.maxAngularVelocity = 30f;

        inputThrust = Vector3.zero;
        inputTorque = Vector3.zero;
        currentFluidDensity = 1f;
    }
    
    void FixedUpdate() {
        Vector3 thrust = ThrustFromInput();
        Vector3 friction = currentFluidDensity * (shipModelInstance.linearFrictionMatrix * localVelocity);
        rigidbody.AddRelativeForce(thrust + friction, ForceMode.Impulse);

        Vector3 torque = TorqueFromInput();
        Vector3 rotFromSpeed = currentFluidDensity * (shipModelInstance.linearToRotationMatrix * localVelocity);
        Vector3 rotFriction = currentFluidDensity * (shipModelInstance.rotationalFrictionMatrix * localAngularVelocity);
        rigidbody.AddRelativeTorque(torque + rotFromSpeed + rotFriction, ForceMode.Impulse);
    }

    private Vector3 ThrustFromInput() {
        return new Vector3(
            (Mathf.Atan(Mathf.Abs(inputThrust.x)) / Mathf.PI) * (inputThrust.x > 0f ? shipModelInstance.maxThrust.x : shipModelInstance.minThrust.x),
            (Mathf.Atan(Mathf.Abs(inputThrust.y)) / Mathf.PI) * (inputThrust.y > 0f ? shipModelInstance.maxThrust.y : shipModelInstance.minThrust.y),
            (Mathf.Atan(Mathf.Abs(inputThrust.z)) / Mathf.PI) * (inputThrust.z > 0f ? shipModelInstance.maxThrust.z : shipModelInstance.minThrust.z));
    }

    private Vector3 TorqueFromInput() {
        float normalizedSpeed = localVelocity.z / shipModelInstance.maxSpeed;
        return new Vector3(
            (Mathf.Atan(inputTorque.x) / Mathf.PI) * (shipModelInstance.flatTorque.x + currentFluidDensity * shipModelInstance.torqueProfile[0].Evaluate(normalizedSpeed)),
            (Mathf.Atan(inputTorque.y) / Mathf.PI) * (shipModelInstance.flatTorque.y + currentFluidDensity * shipModelInstance.torqueProfile[1].Evaluate(normalizedSpeed)),
            (Mathf.Atan(inputTorque.z) / Mathf.PI) * (shipModelInstance.flatTorque.z + currentFluidDensity * shipModelInstance.torqueProfile[2].Evaluate(normalizedSpeed)));
    }

}
