using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawningZone : MonoBehaviour {

    private new Collider collider;
    public int factionIndex;

    private void Start () {
        collider = GetComponent<Collider>();
    }

    public void RespawnShip (Ship ship) {
        ship.status.ResetStatus();
        ship.transform.SetPositionAndRotation(GetSpawnPoint(), transform.rotation);
    }

    private Vector3 GetSpawnPoint () {
        return transform.TransformPoint(Extension.Mathf.GetRandomColliderPoint(collider));
    }
}
