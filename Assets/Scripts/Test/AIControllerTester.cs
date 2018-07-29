using UnityEngine;

public class AIControllerTester : MonoBehaviour {

    AIController aiController;
    AIController.AIControllerInstance instance;
    Ship ship;

    [SerializeField] private float desiredDistance = 30f;
    [SerializeField] private float desiredAccuracy = 0.5f;

    private Transform target;

	void Start () {
        target = GameManager.instance.GetComponent<TestManager>().target;

        ship = GetComponent<Ship>();
        this.aiController = ship.controller.model as AIController;
        instance = GetComponent<Ship>().controller.instance as AIController.AIControllerInstance;
        instance.target = target;
    }

    private void Update () {
        instance.desiredDistance = desiredDistance;
        aiController.accuracy = desiredAccuracy;
    }

    void OnDrawGizmos () {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(instance.targetPosition, 1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(instance.targetAim, 1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(ship.transform.position, ship.transform.position + ship.transform.forward * 100f);
    }

}
