﻿using System;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour, IDestructible {
    new public string name;

    public ShipModel model;
    public ShipStatus status;
    public ShipEngine engine;
    public ShipDynamics dynamics;
    public ShipController controller;
    public ShipAbilities abilities;
    
    public Team team;

    [SerializeField] private List<Transform> weaponTransforms;

    public List<GameObject> weaponSystems { get; private set; }

    public bool isDestroyed { get; private set; } = false;
    
    public event EventHandler<IDestructible> OnDamage;
    public event EventHandler<IDestructible> OnDestruction;

    #region Unity events
    public void Start () {
        ApplyFactionColor();

        status.OnHealthChanged += OnHealthChanged;
        status.OnDeath += OnDeath;

        abilities.OnStart(this);
    }

    private void Update () {
        controller.OnUpdate();
        engine.OnUpdate();
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

    #region initialization
    public void ResetModels () {
        // Instantiate all non shared data
        status = new ShipStatus(this);
        engine = new ShipEngine(this);
        dynamics = new ShipDynamics(this);
        SetupWeapons();
    }

    public void SetControllerModel (ShipControllerModel controllerModel) {
        controller = new ShipController(this, controllerModel);
    }

    private void SetupWeapons () {
        if (model.weaponSystems.Count > weaponTransforms.Count)
            Debug.LogError("Too many weapon systems on this ship", gameObject);

        int weaponCount = Mathf.Min(this.model.weaponSystems.Count, this.weaponTransforms.Count);
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
            GameObjectUtils.SetColorRecursive(modelTransform, team.faction.primaryColor, team.faction.secondaryColor);
    }
    #endregion

    private void OnDeath (object sender, Ship ship) {
        // Stop all movement
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        // Deactivate object and update destruction state
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
        GameManager.GetSpawningZone(team.faction).RespawnShip(this);
        gameObject.SetActive(true);
        isDestroyed = false;
    }

    #region IDestructible
    public bool Damage (float damage) {
        return status.Damage(damage);
    }

    public void Destroy () {
        if (!isDestroyed)
            status.Destroy();
    }
    #endregion
}
