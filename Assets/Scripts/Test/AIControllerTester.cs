using UnityEngine;

public class AIControllerTester : MonoBehaviour {

    AIShipController aiShipController;
    AIShipController.AIShipControllerInstance instance;
    Ship ship;

    public float anticipation = 0.01f;

    private Transform target;

	void Start () {
        target = GameManager.instance.GetComponent<TestManager>().target;

        ship = GetComponent<Ship>();
        this.aiShipController = ship.controller.model as AIShipController;
        instance = GetComponent<Ship>().controller.instance as AIShipController.AIShipControllerInstance;
        instance.target = target;
    }

    private void Update () {
        instance.anticipationFactor = anticipation;
    }

    private void OnDrawGizmos () {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(target.position, target.position + instance.targetVelocity);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(target.position, target.position + instance.targetAcceleration);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(instance.targetPoint, 1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(ship.transform.position, ship.transform.position + 10f * ship.transform.TransformVector (ship.engine.inputThrust));
    }
}
