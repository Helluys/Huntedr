using System;
using UnityEngine;

/// <summary>
/// Generic abstract base class for target pickers.
/// Called by Abilities to select a target. Should be instantiated by the Ability first.
/// Abilities register to the OnTargetPicked to be notified of target selection.
/// Public interface includes StartPicking and CancelPicking, which call the implementation methods OnStartPicking and
/// OnCancelPicking. Implementation should call EndPicking to trigger the OnTargetPicked event.
/// </summary>
/// <typeparam name="T">The selected target type</typeparam>
public abstract class TargetPicker<T> : ScriptableObject {

    public event EventHandler<T> OnTargetPicked;

    public bool isPicking { get; private set; }

    protected Ship caster;

    public void StartPicking (Ship casterShip) {
        isPicking = true;
        caster = casterShip;
        OnStartPicking();
    }

    public void CancelPicking () {
        isPicking = false;
        OnCancelPicking();
    }

    protected abstract void OnStartPicking ();

    protected abstract void OnCancelPicking ();

    protected void EndPicking (T target) {
        isPicking = false;
        if (target != null)
            OnTargetPicked?.Invoke(this, target);
    }

}