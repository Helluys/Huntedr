using UnityEngine;

public class AIControllerTester : MonoBehaviour {

    AIControllerModel aiController;
    AIControllerModel.AIControllerInstance instance;
    Ship ship;

    [SerializeField] private float desiredDistance = 100f;
    [SerializeField] [Range(0f, 1f)] private float desiredAccuracy = 1f;

    public LowLevelObjective objective;

    private Transform target;

    void Start () {
        ship = GetComponent<Ship>();
        this.aiController = ship.controller.model as AIControllerModel;
        instance = GetComponent<Ship>().controller.instance as AIControllerModel.AIControllerInstance;
        instance.SetObjective(objective);
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
