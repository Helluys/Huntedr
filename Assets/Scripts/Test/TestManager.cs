using UnityEngine;

public class TestManager : MonoBehaviour {
    public Transform target;

    [SerializeField] private Ship testShip;

    void Start () {
        testShip = GameManager.instance.shipList[0];

        if (GameManager.instance.shipList.Count > 1)
            target = GameManager.instance.shipList[1].transform;

        testShip.gameObject.AddComponent<AIControllerTester>();
    }
}
