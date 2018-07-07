using System;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectDetector : MonoBehaviour {

    [SerializeField] Transform trackedTransform;

    public event EventHandler<Collider> OnObjectDetected;
    public event EventHandler<Collider> OnObjectLost;

    private void Start () {
        transform.parent = null;
    }

    private void FixedUpdate () {
        transform.position = trackedTransform.position;
        transform.rotation = trackedTransform.rotation;
    }

    private void OnTriggerEnter (Collider other) {
        OnObjectDetected?.Invoke(this, other);
    }

    private void OnTriggerExit (Collider other) {
        OnObjectLost?.Invoke(this, other); 
    }

    public void SetTrackedTransform(Transform newTrackedTransform) {
        trackedTransform = newTrackedTransform;
    }
}