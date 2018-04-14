using UnityEngine;

[RequireComponent(typeof(ShipDynamics))]
public class ShipController : MonoBehaviour {
    ShipDynamics shipDynamics;

    void Start() {
        shipDynamics = GetComponent<ShipDynamics>();
    }

    void Update() {
        shipDynamics.inputThrust = Input.GetAxis("Thrust") * Vector3.forward;
        shipDynamics.inputTorque = 
            Input.GetAxis("Pitch") * Vector3.up +
            Input.GetAxis("Yaw") * Vector3.right + 
            Input.GetAxis("Roll") * Vector3.forward;
    }
}