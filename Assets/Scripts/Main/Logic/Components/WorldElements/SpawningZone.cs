using UnityEngine;

using Utilities;

[RequireComponent(typeof(Collider))]
public class SpawningZone : MonoBehaviour {

    private new Collider collider;
    public Faction faction;

    private void Start () {
        GameManager.AddSpawningZone(this);
        collider = GetComponent<Collider>();
    }

    public void RespawnShip (Ship ship) {
        ship.status.ResetStatus();
        ship.transform.SetPositionAndRotation(GetSpawnPoint(), transform.rotation);
    }

    private Vector3 GetSpawnPoint () {
        return transform.TransformPoint(MathUtils.GetRandomColliderPoint(collider));
    }
}
