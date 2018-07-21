using UnityEngine;

public class ObjectTracker : MonoBehaviour {

    [SerializeField] Transform trackedTransform;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 rotationOffset;

    private void Start () {
        transform.parent = null;
    }

    private void FixedUpdate () {
        if (trackedTransform) {
            transform.position = trackedTransform.position + trackedTransform.TransformVector(offset);
            transform.rotation = Quaternion.Euler(rotationOffset) * trackedTransform.rotation;
        }
    }

    public void SetTrackedTransform (Transform newTrackedTransform, bool keepOffset = false) {
        trackedTransform = newTrackedTransform;
        if (keepOffset) {
            offset = trackedTransform.InverseTransformVector (transform.position - trackedTransform.position);
            rotationOffset = (transform.rotation * Quaternion.Inverse(trackedTransform.rotation)).eulerAngles;
        }
    }

}
