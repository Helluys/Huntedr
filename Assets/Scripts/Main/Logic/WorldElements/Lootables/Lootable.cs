using System.Collections.Generic;
using UnityEngine;

public abstract class Lootable : MonoBehaviour {

    public abstract void PickUp (Ship ship);

    private List<Ship> collidedList = new List<Ship>();

    private void OnTriggerEnter (Collider other) {
        Ship ship = other.attachedRigidbody.GetComponent<Ship>();

        if (ship && !collidedList.Contains(ship)) {
            PickUp(ship);
            collidedList.Add(ship);
        }
    }

    private void OnTriggerExit (Collider other) {
        Ship ship = other.attachedRigidbody.GetComponent<Ship>();

        if (ship && collidedList.Contains(ship))
            collidedList.Remove(ship);
    }
}
