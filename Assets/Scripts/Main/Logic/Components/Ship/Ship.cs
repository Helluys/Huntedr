using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour {
    [SerializeField] private string _shipName;
    private string shipName { get { return _shipName; } }

    public ShipModel model;
    public ShipStatus status;
    public ShipDynamics dynamics;
    public ShipController controller;

    public Faction faction;

    [SerializeField] private List<Transform> weaponTransforms;
    public List<WeaponSystem.IInstance> weaponSystems { get; private set; }

    private ShipController.IInstance controllerInstance;

    public void Start () {
        // Instantiate all non shared data
        status = new ShipStatus(this);
        dynamics = new ShipDynamics(this);
        controllerInstance = controller.CreateInstance(this);
        SetupWeapons();

        ApplyFactionColor();

        status.OnDeath += OnDeath;
    }

    private void SetupWeapons () {
        if (model.weaponSystems.Count > weaponTransforms.Count)
            Debug.LogError("Too many weapon systems on this ship", gameObject);

        int weaponCount = Mathf.Min(model.weaponSystems.Count, weaponTransforms.Count);
        weaponSystems = new List<WeaponSystem.IInstance>(weaponCount);
        for (int i = 0; i < weaponCount; i++)
            weaponSystems.Add(model.weaponSystems[i].CreateInstance(weaponTransforms[i], this));
    }

    private void ApplyFactionColor () {
        Transform modelTransform = transform.Find("Model");
        if(modelTransform != null) {

            Renderer modelRenderer = modelTransform.GetComponent<Renderer>();
            if(modelRenderer != null) {
                modelRenderer.material.color = faction.color;
            }
        }
    }

    private void OnDeath (object sender, Ship e) {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
        Invoke("Respawn", 2f);
    }

    private void Respawn() {
        GameManager.GetSpawningZone(faction).RespawnShip(this);
        gameObject.SetActive(true);
    }

    private void Update () {
        controllerInstance.OnUpdate();
    }

    private void FixedUpdate () {
        dynamics.OnFixedUpdate();
    }

    private void OnCollisionEnter (Collision collision) {
        status.Damage(collision.impulse.magnitude);
    }

}
