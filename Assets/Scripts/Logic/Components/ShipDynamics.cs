using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipDynamics : MonoBehaviour {

    public Vector3 inputThrust { get; set; }
    public Vector3 inputTorque { get; set; }

    public float currentFluidDensity { get; set; }

    private Vector3 localVelocity { get { return transform.InverseTransformVector(rigidbody.velocity); } }
    private Vector3 localAngularVelocity { get { return transform.InverseTransformVector(rigidbody.angularVelocity); } }

    [SerializeField] private ShipModel shipModel;
    private new Rigidbody rigidbody;
    
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = shipModel.mass;
        rigidbody.inertiaTensor = shipModel.inertiaTensor;
        rigidbody.inertiaTensorRotation = Quaternion.identity;
        rigidbody.maxAngularVelocity = 30f;

        inputThrust = Vector3.zero;
        inputTorque = Vector3.zero;
        currentFluidDensity = 1f;
    }
    
    void FixedUpdate() {
        Vector3 thrust = ThrustFromInput();
        Vector3 friction = currentFluidDensity * (shipModel.linearFrictionMatrix * localVelocity);
        rigidbody.AddRelativeForce(thrust + friction, ForceMode.Impulse);

        Vector3 torque = TorqueFromInput();
        Vector3 rotFromSpeed = currentFluidDensity * (shipModel.linearToRotationMatrix * localVelocity);
        Vector3 rotFriction = currentFluidDensity * (shipModel.rotationalFrictionMatrix * localAngularVelocity);
        rigidbody.AddRelativeTorque(torque + rotFromSpeed + rotFriction, ForceMode.Impulse);
    }

    private Vector3 ThrustFromInput() {
        return new Vector3(
            Mathf.Abs(inputThrust.x) * (inputThrust.x > 0f ? shipModel.maxThrust.x : shipModel.minThrust.x),
            Mathf.Abs(inputThrust.y) * (inputThrust.y > 0f ? shipModel.maxThrust.y : shipModel.minThrust.y),
            Mathf.Abs(inputThrust.z) * (inputThrust.z > 0f ? shipModel.maxThrust.z : shipModel.minThrust.z));
    }

    private Vector3 TorqueFromInput() {
        float normalizedSpeed = localVelocity.z / shipModel.maxSpeed;
        return new Vector3(
            inputTorque.x * (shipModel.flatTorque.x + currentFluidDensity * shipModel.torqueProfile[0].Evaluate(normalizedSpeed)),
            inputTorque.y * (shipModel.flatTorque.x + currentFluidDensity * shipModel.torqueProfile[1].Evaluate(normalizedSpeed)),
            inputTorque.z * (shipModel.flatTorque.x + currentFluidDensity * shipModel.torqueProfile[2].Evaluate(normalizedSpeed)));
    }
}
