using System;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectDetector : ObjectTracker {

    public event EventHandler<Collider> OnObjectDetected;
    public event EventHandler<Collider> OnObjectLost;

    private void OnTriggerEnter (Collider other) {
        OnObjectDetected?.Invoke(this, other);
    }

    private void OnTriggerExit (Collider other) {
        OnObjectLost?.Invoke(this, other); 
    }
}