using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : ObjectTracker {

    private void Start () {
        SetTrackedTransform(GameManager.instance.playerList[0].transform);
    }

}
