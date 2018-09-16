using UnityEngine;

public class LowLevelObjective {

    public enum Type {
        TargetShip, TargetObject, MoveToPoint
    }

    public readonly Type type;
    public readonly Transform target;
    public readonly Vector3 point;

    private LowLevelObjective (Type type, Transform target, Vector3 point) {
        this.type = type;
        this.target = target;
        this.point = point;
    }

    public static LowLevelObjective TargetShip (Ship ship) {
        return new LowLevelObjective(Type.TargetShip, ship.transform, Vector3.zero);
    }

    public static LowLevelObjective TargetObject (GameObject go) {
        return new LowLevelObjective(Type.TargetObject, go.transform, Vector3.zero);
    }

    public static LowLevelObjective MoveToPoint (Vector3 point) {
        return new LowLevelObjective(Type.MoveToPoint, null, point);
    }
}
