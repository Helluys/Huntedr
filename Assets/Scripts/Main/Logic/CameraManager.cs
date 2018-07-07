using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public Vector3 rotation;

    private void Start () {
        target = GameManager.instance.playerList[0].transform;
    }

    private void Update () {
        if (target != null) {
            transform.position = target.position + target.TransformVector(offset);
            transform.rotation = Quaternion.Euler(rotation) * target.rotation;
        }
    }
}
