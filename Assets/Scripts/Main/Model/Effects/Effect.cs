using System;
using System.Collections;
using UnityEngine;

public abstract class Effect : ScriptableObject {
    [Serializable]
    public enum EndCondition {
        OneFrame,
        FixedTime,
        Unlimited
    }

    public enum RepeatType {
        AtStartup, EveryFrame
    }

    new public string name;

    public float duration;

    public EndCondition endCondition;

    public RepeatType repeatType;

    private Coroutine repeatCoroutine;

    protected Ship target;

    public bool active { get; protected set; }

    public event EventHandler<Effect> OnDeactivation;

    public void Activate (Ship ship) {
        if (active) return;

        target = ship;

        switch (endCondition) {
            case EndCondition.OneFrame:
                Apply(ship);
                break;
            case EndCondition.FixedTime:
                var endTime = Time.time + duration;
                repeatCoroutine = ship.StartCoroutine(PerformEffect(endTime));
                active = true;
                break;
            case EndCondition.Unlimited:
                this.repeatCoroutine = ship.StartCoroutine(PerformEffect(Mathf.Infinity));
                active = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Deactivate () {
        if (!active) return;

        active = false;
        target.StopCoroutine(repeatCoroutine);
        repeatCoroutine = null;
        OnDeactivation?.Invoke(this, this);
    }

    protected abstract void Apply (Ship ship);

    private IEnumerator PerformEffect (float endTime) {
        switch (repeatType) {
            case RepeatType.AtStartup:
                Apply(target);
                yield return new WaitForSeconds(endTime - Time.time);
                break;
            case RepeatType.EveryFrame:
                while (Time.time < endTime) {
                    Apply(target);
                    yield return null;
                }
                break;
            default:
                break;
        }


        Deactivate();
    }
}