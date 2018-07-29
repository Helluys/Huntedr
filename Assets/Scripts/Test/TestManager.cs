using UnityEngine;

public class TestManager : MonoBehaviour {
    public Transform target;

    [SerializeField] private Ship testShip;

    [SerializeField] private ControllerType testedController;

    public enum ControllerType {
        AITest, SlidingMode
    }

    void Start () {
        testShip = GameManager.instance.shipList[0];

        if (GameManager.instance.shipList.Count > 1)
            target = GameManager.instance.shipList[1].transform;
        Debug.Log("testmanager");
        switch (testedController) {
            case ControllerType.AITest:
                testShip.gameObject.AddComponent<AIControllerTester>();
                break;
            case ControllerType.SlidingMode:
                testShip.gameObject.AddComponent<SlidingModeControllerTester>();
                break;
        }
    }
}
