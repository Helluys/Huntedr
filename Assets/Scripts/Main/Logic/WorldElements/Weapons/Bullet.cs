using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] float despawnTime = 5f;

    private void Start () {
        Destroy(gameObject, despawnTime);
    }

    private void OnCollisionEnter (Collision collision) {
        IDestructible hitTarget = collision.rigidbody?.GetComponent<IDestructible>();
        hitTarget?.Damage(collision.impulse.magnitude);
        Destroy(gameObject);
    }
}
