using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour {

    [SerializeField] private float explosionTimeout = 5f;
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float explosionForce = 50f;

    [SerializeField] private GameObject explosionEffectPrefab;

    [SerializeField] private float speed = 15f;
    [Tooltip("In degrees per second")] [SerializeField] private float maxAngularSpeed = 180f;

    public Transform target;

    private new Rigidbody rigidbody;

    private bool exploded;

    private void Start () {
        rigidbody = GetComponent<Rigidbody>();
        if (explosionTimeout > 0f)
            Invoke("Explode", explosionTimeout);
    }

    private void FixedUpdate () {
        if (target != null) {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            this.maxAngularSpeed * Time.fixedDeltaTime);
        }
        rigidbody.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter (Collision collision) {
        Explode();
    }

    private void Explode () {
        if (exploded)
            return;

        // Retrieve all hit rigidbodies (avoid duplicates from several colliders on same body)
        IDictionary<Rigidbody, Vector3> hitBodies = new Dictionary<Rigidbody, Vector3>();
        foreach (Collider hitCollider in Physics.OverlapSphere(transform.position, explosionRadius)) {
            Vector3 closestPoint = hitCollider.ClosestPoint(transform.position);
            Rigidbody attachedRigidbody = hitCollider.attachedRigidbody;

            if (attachedRigidbody != null) {
                Ray ray = new Ray(transform.position, (hitCollider.transform.TransformPoint(hitCollider.bounds.center) - transform.position).normalized);
                RaycastHit hitInfo = new RaycastHit();
                Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore);

                Debug.Log(hitInfo.collider?.name);
                if (hitInfo.collider?.attachedRigidbody == attachedRigidbody && hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Shield")) {
                    if (!hitBodies.ContainsKey(attachedRigidbody))
                        hitBodies.Add(attachedRigidbody, closestPoint);
                    else if (closestPoint.magnitude < hitBodies[attachedRigidbody].magnitude)
                        hitBodies[attachedRigidbody] = closestPoint;
                }
            }
        }

        // Apply damage and force to hit rigidbodies
        foreach (KeyValuePair<Rigidbody, Vector3> hitBody in hitBodies) {
            float distance = (hitBody.Value - transform.position).magnitude / explosionRadius;
            float damage = explosionForce * Mathf.Max(1f - distance, 0f);
            hitBody.Key.AddForce(damage * (hitBody.Key.transform.position - transform.position));

            IDestructible hitTarget = hitBody.Key.GetComponent<IDestructible>();
            hitTarget?.Damage(damage);
        }

        // Instantiate effect
        Instantiate(explosionEffectPrefab, transform.position, transform.rotation);

        exploded = true;
        Destroy(gameObject);
    }
}
