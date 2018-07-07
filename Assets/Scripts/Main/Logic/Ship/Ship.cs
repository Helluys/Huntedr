using System;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour, IDestructible {
    new public string name;

    public ShipModel model;
    public ShipStatus status;
    public ShipDynamics dynamics;
    public ShipController controller;

    public Faction faction;

    [SerializeField] private List<Transform> weaponTransforms;
    public List<GameObject> weaponSystems { get; private set; }

    public bool isDestroyed { get; private set; } = false;

    private ShipController.IInstance controllerInstance;

    public event EventHandler<IDestructible> OnDamage;
    public event EventHandler<IDestructible> OnDestruction;

    #region Unity events
    public void Start () {
        ApplyFactionColor();

        status.OnHealthChanged += OnHealthChanged;
        status.OnDeath += OnDeath;
    }

    public void ResetModels () {
        // Instantiate all non shared data
        status = new ShipStatus(this);
        dynamics = new ShipDynamics(this);
        controllerInstance = controller.CreateInstance(this);
        SetupWeapons();
    }

    private void Update () {
        controllerInstance.OnUpdate();
    }

    private void FixedUpdate () {
        dynamics.OnFixedUpdate();
    }

    private void OnCollisionEnter (Collision collision) {
        // Avoid applying damage twice on collision with Bullet
        if (collision.rigidbody == null || collision.rigidbody.gameObject.GetComponent<Bullet>() == null)
            Damage(collision.impulse.magnitude);
    }
    #endregion

    private void SetupWeapons () {
        if (model.weaponSystems.Count > weaponTransforms.Count)
            Debug.LogError("Too many weapon systems on this ship", gameObject);

        int weaponCount = Mathf.Min(model.weaponSystems.Count, weaponTransforms.Count);
        weaponSystems = new List<GameObject>(weaponCount);
        for (int i = 0; i < weaponCount; i++) {
            GameObject weaponSystemGO = Instantiate(model.weaponSystems[i], weaponTransforms[i]);
            weaponSystemGO.GetComponent<WeaponSystem>().Initialize(this);
            this.weaponSystems.Add(weaponSystemGO);
        }
    }

    private void ApplyFactionColor () {
        Transform modelTransform = transform.Find("Model");
        if (modelTransform != null)
            GameObjectUtils.SetColorRecursive(modelTransform, faction.primaryColor, faction.secondaryColor);
    }

    private void OnDeath (object sender, Ship e) {
        // Stop all movement
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        // Deactivate object and udate destruction state
        gameObject.SetActive(false);
        isDestroyed = true;
        if (OnDestruction != null)
            OnDestruction(this, this);

        // Prepare respawn
        Invoke("Respawn", 2f);
    }

    private void OnHealthChanged (object sender, float healthDelta) {
        if (healthDelta < 0f && OnDamage != null)
            OnDamage(this, this);
    }

    public void Respawn () {
        GameManager.GetSpawningZone(faction).RespawnShip(this);
        gameObject.SetActive(true);
        isDestroyed = false;
    }

    #region IDestructible
    public bool Damage (float damage) {
        return status.Damage(damage);
    }

    public void Destroy () {
        status.Destroy();
    }
    #endregion
}
