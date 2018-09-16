using UnityEngine;

public class TestManager : MonoBehaviour {
    public Transform targetShip;

    [SerializeField] private Ship testShip;
    [SerializeField] private LowLevelObjective objective;

    void Start () {
        testShip = GameManager.instance.shipList[0];

        if (GameManager.instance.shipList.Count > 1)
            targetShip = GameManager.instance.shipList[1].transform;

        testShip.gameObject.AddComponent<AIControllerTester>().objective = this.objective;
    }
}
