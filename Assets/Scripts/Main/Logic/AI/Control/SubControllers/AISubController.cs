using System;

public abstract class AISubController {
    public float accuracy;

    public event EventHandler<AISubController> OnObjectiveCompleted;

    public abstract SlidingModeControl.Target ComputeTarget ();

    protected void ObjectiveCompleted () {
        OnObjectiveCompleted?.Invoke(this, this);
    }
}
