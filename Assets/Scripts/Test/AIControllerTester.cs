using UnityEngine;

public class AIControllerTester : MonoBehaviour {

    AIController aiController;
    AIController.AIControllerInstance instance;
    Ship ship;

    [SerializeField] private float desiredDistance = 100f;
    [SerializeField] [Range(0f, 1f)] private float desiredAccuracy = 1f;

    public AIController.AIControllerInstance.Objective objective;

    private Transform target;

    void Start () {
        switch (objective) {
            case AIController.AIControllerInstance.Objective.TargetShip:
                target = GameManager.instance.GetComponent<TestManager>().targetShip;
                break;
            case AIController.AIControllerInstance.Objective.MoveToPoint:
                target = GameObject.Find("Target").transform;
                break;
        }

        ship = GetComponent<Ship>();
        this.aiController = ship.controller.model as AIController;
        instance = GetComponent<Ship>().controller.instance as AIController.AIControllerInstance;
        instance.SetObjective(objective, target);
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
