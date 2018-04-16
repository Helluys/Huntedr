using UnityEngine;

[RequireComponent(typeof(ShipDynamics))]
public class ShipController : MonoBehaviour {

    private ShipDynamics shipDynamics;
    
    [SerializeField] private float mouseSensitivity;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        shipDynamics = GetComponent<ShipDynamics>();
    }

    void Update() {
        shipDynamics.inputThrust = 
            Input.GetAxis("HorizontalThrust") * Vector3.right + 
            Input.GetAxis("VerticalThrust") *   Vector3.up + 
            Input.GetAxis("Thrust") *           Vector3.forward;

        Vector3 cursorPosition = (-Input.GetAxis("Pitch") * Vector3.right + Input.GetAxis("Yaw") * Vector3.up) * mouseSensitivity;
        shipDynamics.inputTorque = cursorPosition + Input.GetAxis("Roll") * Vector3.forward;

        shipDynamics.inputDrag = Input.GetAxis("Cushion");
        shipDynamics.inputAngularDrag = Input.GetAxis("Stabilize");
    }

}
