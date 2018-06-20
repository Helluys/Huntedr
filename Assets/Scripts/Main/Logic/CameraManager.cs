using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public Vector3 rotation;

    private void Update () {
        transform.position = target.position + target.TransformVector(offset);
        transform.rotation = Quaternion.Euler(rotation) * target.rotation;
    }
}
