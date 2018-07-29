using UnityEngine;

public class SlidingModeControllerTester : MonoBehaviour {

    SlidingModeController slidingModeController;
    SlidingModeController.SlidingModeControllerInstance instance;
    Ship ship;

    private Transform target;

	void Start () {
        target = GameManager.instance.GetComponent<TestManager>().target;
        Debug.Log("tester " + target);

        ship = GetComponent<Ship>();
        this.slidingModeController = ship.controller.model as SlidingModeController;
        instance = GetComponent<Ship>().controller.instance as SlidingModeController.SlidingModeControllerInstance;
        instance.target = target;
    }

}
