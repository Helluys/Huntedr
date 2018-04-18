using UnityEngine;

[RequireComponent(typeof(ShipStatus))]
[RequireComponent(typeof(ShipDynamics))]
public class Ship : MonoBehaviour {
    [SerializeField] private string _shipName;
    private string shipName { get { return _shipName; } }

    [SerializeField] private ShipModel shipModel;

    void Start () {
    }
}
