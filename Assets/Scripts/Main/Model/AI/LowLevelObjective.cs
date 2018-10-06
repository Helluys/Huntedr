using UnityEngine;

[System.Serializable]
public class LowLevelObjective {

    [System.Serializable]
    public enum Type {
        Destroy, MoveToPoint
    }

    public Type type;
    public IDestructible target;
    public Vector3 point;

    private System.Predicate<Ship> objectiveReached;

    private LowLevelObjective (Type type, IDestructible target, Vector3 point, System.Predicate<Ship> predicate) {
        this.type = type;
        this.target = target;
        this.point = point;
        this.objectiveReached = predicate;
    }

    public static LowLevelObjective Destroy (IDestructible destructible) {
        return new LowLevelObjective(Type.Destroy, destructible, Vector3.zero, s => destructible.isDestroyed);
    }

    public static LowLevelObjective MoveToPoint (Vector3 point, float distance) {
        return new LowLevelObjective(Type.MoveToPoint, null, point, s => (s.transform.position - point).magnitude < distance);
    }

    public static LowLevelObjective GetInSight (Transform transform) {
        return new LowLevelObjective(Type.MoveToPoint, null, transform.position, s => GameObjectUtils.CanSee(s, transform.gameObject));
    }

    public bool ObjectiveReached(Ship ship) {
        return objectiveReached(ship);
    }
}
