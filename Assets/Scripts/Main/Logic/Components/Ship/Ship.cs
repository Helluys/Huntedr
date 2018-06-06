using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour {
    [SerializeField] private string _shipName;
    private string shipName { get { return _shipName; } }

    public ShipModel shipModel;
    public ShipStatus shipStatus;
    public ShipDynamics shipDynamics;
    public ShipController shipController;

    [SerializeField] private List<Transform> weaponTransforms;
    public List<WeaponSystem.IInstance> weaponSystems { get; private set; }

    private ShipController.IInstance shipControllerInstance;

    public void Start () {
        shipStatus = new ShipStatus(this);
        shipDynamics = new ShipDynamics(this);
        shipControllerInstance = shipController.CreateInstance(this);

        if (shipModel.weaponSystems.Count > weaponTransforms.Count)
            Debug.LogError("Too many weapon systems on this ship", gameObject);

        int weaponCount = Mathf.Min(shipModel.weaponSystems.Count, weaponTransforms.Count);
        weaponSystems = new List<WeaponSystem.IInstance>(weaponCount);
        for (int i = 0; i < weaponCount; i++)
            weaponSystems.Add(shipModel.weaponSystems[i].CreateInstance(weaponTransforms[i], this));

    }

    private void Update () {
        shipControllerInstance.OnUpdate();
    }

    private void FixedUpdate () {
        shipDynamics.OnFixedUpdate();
    }

    private void OnCollisionEnter (Collision collision) {
        shipStatus.Damage(collision.impulse.magnitude);
    }

}
