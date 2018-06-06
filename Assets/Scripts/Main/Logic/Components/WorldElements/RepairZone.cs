using UnityEngine;

public class RepairZone : MonoBehaviour {

    [SerializeField] private float repairRate;

    private void OnTriggerStay (Collider other) {
        Ship ship = other.attachedRigidbody.GetComponent<Ship>();
        
        if (ship != null) {
            ship.shipStatus.Repair(repairRate * Time.fixedDeltaTime);
        }
    }
}
