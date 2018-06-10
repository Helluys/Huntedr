using System;
using System.Collections;
using UnityEngine;

public abstract class Effect : ScriptableObject {

    [Serializable]
    public enum Type {
        ONE_SHOT, FIXED_TIME, UNLIMITED
    }

    public Type type;
    public float duration;

    public bool active {get; protected set; }

    protected Ship target;

    private Coroutine repeatCoroutine = null;

    public void Activate (Ship ship) {
        if (!active) {
            target = ship;

            switch (type) {
                case Type.ONE_SHOT:
                    Apply(ship);
                    break;
                case Type.FIXED_TIME:
                    float endTime = Time.time + duration;
                    repeatCoroutine = ship.StartCoroutine(RepeatEffect(endTime));
                    active = true;
                    break;
                case Type.UNLIMITED:
                    repeatCoroutine = ship.StartCoroutine(RepeatEffect(Mathf.Infinity));
                    active = true;
                    break;
            }
        }
    }

    public void Deactivate () {
        if (active) {
            active = false;
            target.StopCoroutine(repeatCoroutine);
            repeatCoroutine = null;
        }
    }

    protected abstract void Apply (Ship ship);

    private IEnumerator RepeatEffect (float endTime) {
        while (Time.time < endTime) {
            Apply(target);
            yield return null;
        }
        Deactivate();
    }
}
