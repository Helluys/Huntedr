using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] float despawnTime = 5f;

    private void Start () {
        Destroy(gameObject, despawnTime);
    }

    private void OnCollisionEnter (Collision collision) {
        IDestructible hitTarget = null;
        if (collision.rigidbody != null)
            hitTarget = collision.rigidbody.GetComponent<IDestructible>();

        if (hitTarget != null)
            hitTarget.Damage(collision.impulse.magnitude);

        Destroy(gameObject);
    }
}
