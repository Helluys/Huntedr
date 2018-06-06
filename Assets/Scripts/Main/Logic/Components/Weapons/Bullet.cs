using UnityEngine;

public class Bullet : MonoBehaviour {

    private void Start () {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter (Collision collision) {
        Ship hitShip = null;
        if (collision.rigidbody != null)
            hitShip = collision.rigidbody.GetComponent<Ship>();

        if (hitShip != null)
            hitShip.shipStatus.Damage(5);

        Destroy(gameObject);
    }
}
