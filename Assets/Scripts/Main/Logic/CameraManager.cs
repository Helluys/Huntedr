using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : ObjectTracker {

    private void Start () {
        if (GameManager.instance.playerList.Count != 0)
            SetTrackedTransform(GameManager.instance.playerList[0].transform);
    }

}
