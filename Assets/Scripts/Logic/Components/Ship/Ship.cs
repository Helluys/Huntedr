using UnityEngine;

[RequireComponent(typeof(ShipStatus))]
[RequireComponent(typeof(ShipDynamics))]
public class Ship : MonoBehaviour {
    [SerializeField] private string _shipName;
    private string shipName { get { return _shipName; } }

    [SerializeField] private ShipModel shipModel;

    private ShipStatus shipStatus;

    private void Start () {
        shipStatus = GetComponent<ShipStatus>();
    }

    private void OnCollisionEnter (Collision collision) {
        shipStatus.Damage(collision.impulse.magnitude);
    }

}
