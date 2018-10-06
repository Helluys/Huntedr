using UnityEngine;

public class TestManager : MonoBehaviour {
    public Ship targetShip;

    [SerializeField] private Ship testShip;
    [SerializeField] private LowLevelObjective objective;

    private void Start () {
        testShip = GameManager.instance.shipList[0];

        if (GameManager.instance.shipList.Count > 1) {
            targetShip = GameManager.instance.shipList[1];
            objective.target = targetShip;
        }

        testShip.gameObject.AddComponent<AIControllerTester>().objective = this.objective;
    }
}
