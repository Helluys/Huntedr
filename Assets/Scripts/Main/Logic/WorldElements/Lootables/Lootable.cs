using UnityEngine;

public abstract class Lootable : MonoBehaviour {

    public abstract void PickUp (Ship ship);

    private void OnTriggerEnter (Collider other) {
        Ship ship = other.attachedRigidbody.GetComponent<Ship>();

        if (ship)
            PickUp(ship);
    }
}
