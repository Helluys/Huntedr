using System.Collections.Generic;
using UnityEngine;

public class OneWayCollider : MonoBehaviour {

    [SerializeField] LayerMask layermask;
    private List<Collider> colliders = new List<Collider>();

    private void Start () {
        colliders = new List<Collider>(GetComponents<Collider>());
    }

    private void OnTriggerStay (Collider otherCollider) {
        bool ignore = Vector3.Dot(otherCollider.attachedRigidbody.velocity, transform.forward) > 0f
                    && CheckLayer(otherCollider.gameObject.layer);

        if (ignore)
            foreach (Collider collider in colliders)
                Physics.IgnoreCollision(otherCollider, collider, ignore);
    }

    private bool CheckLayer (int layer) {
        return layermask == (layermask | (1 << layer));
    }

}
