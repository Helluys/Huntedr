using UnityEngine;

public class HighLevelObjective {

    public enum Type {
        DefendTarget, AttackTarget, Scout, Retreat
    }

    public Type type;
    public GameObject target;

    public HighLevelObjective (Type type, GameObject target) {
        this.type = type;
        this.target = target;
    }
}
