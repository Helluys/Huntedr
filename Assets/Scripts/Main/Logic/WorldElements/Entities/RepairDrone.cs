using UnityEngine;

public class RepairDrone : MonoBehaviour {

    public Ship target { get; set; }

    [SerializeField] private float reachDistance = 3f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float repairRate;

    private Vector3 currentVelocity = Vector3.zero;

    void Update () {
        if (target != null) {
            float distance = (target.transform.position - transform.position).magnitude;
            if (distance > reachDistance) {
                transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref currentVelocity, 1f / acceleration, maxSpeed, Time.deltaTime);
            } else {
                target.status.Repair(repairRate * Time.deltaTime);
            }
        }
    }
}
