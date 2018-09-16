using System;
using UnityEngine;

public class Destructible : MonoBehaviour, IDestructible {

    public event EventHandler<IDestructible> OnDamage;
    public event EventHandler<IDestructible> OnDestruction;

    [SerializeField] private DestructionSequence destructionSequence;

    public bool isDestroyed { get; private set; } = false;

    [SerializeField] private float _totalHealth;
    public float totalHealth { get { return _totalHealth; } private set { _totalHealth = value; } }

    [SerializeField] private float _health;
    public float health { get { return _health; } private set { _health = value; } }

    public float healthRatio { get { return health / totalHealth; } }
    
    public void Destroy () {
        isDestroyed = true;
        health = 0f;

        if (OnDestruction != null)
            OnDestruction(this, this);

        StartCoroutine(destructionSequence.DestructionCoroutine(this));
    }

    public bool Damage (float damage) {
        health -= damage;

        if (health <= 0f)
            Destroy();

        if (OnDamage != null)
            OnDamage(this, this);

        return isDestroyed;
    }

    private void OnCollisionEnter (Collision collision) {
        Damage(collision.impulse.magnitude);
    }

}

public interface IDestructible {

    event EventHandler<IDestructible> OnDestruction;
    event EventHandler<IDestructible> OnDamage;

    GameObject gameObject { get; }

    bool Damage (float damage);
    void Destroy ();

    bool isDestroyed { get; }
}