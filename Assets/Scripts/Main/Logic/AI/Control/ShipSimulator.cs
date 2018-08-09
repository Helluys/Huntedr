using UnityEngine;

public class ShipSimulator {

    public Transform target;
    public float anticipationTime;

    public Vector3 simulatedPosition { get; private set; }
    public Vector3 simulatedDirection { get; private set; }

    public Vector3 simulatedVelocity { get; private set; }
    public Vector3 simulatedAngularVelocity { get; private set; }

    private const int SIMULATION_POOL = 30;
    private Vector3[] lastTargetPositions = new Vector3[3];
    private Vector3[] lastTargetDirections = new Vector3[3];

    public ShipSimulator (Transform target, float anticipationTime = 1f) {
        this.target = target;
        this.anticipationTime = anticipationTime;

        lastTargetPositions = new Vector3[SIMULATION_POOL];
        lastTargetDirections = new Vector3[SIMULATION_POOL];
        for (int i = 0; i < lastTargetDirections.Length; i++) {
            lastTargetPositions[i] = Vector3.zero;
            lastTargetDirections[i] = Vector3.zero;
        }
    }

    public void UpdateSimulator () {
        if (target == null)
            return;

        ShiftTargetArrays();

        Vector3[] velocityArray = Extension.Mathf.ArrayDelta(lastTargetPositions, 1f / Time.fixedDeltaTime);
        simulatedVelocity = Extension.Mathf.ArrayAverage(velocityArray);

        Vector3[] angularVelocityArray = Extension.Mathf.ArrayDelta(lastTargetDirections, 1f / Time.fixedDeltaTime);
        simulatedAngularVelocity = Extension.Mathf.ArrayAverage(angularVelocityArray);

        simulatedPosition = target.position;
        simulatedDirection = target.forward;

        float simulationTime = 0f;
        while (simulationTime < anticipationTime) {
            this.simulatedPosition += this.simulatedVelocity * Time.fixedDeltaTime;
            this.simulatedDirection += this.simulatedAngularVelocity * Time.fixedDeltaTime;

            simulationTime += Time.fixedDeltaTime;
        }

    }

    private void ShiftTargetArrays () {
        for (int i = lastTargetPositions.Length - 1; i > 0; i--)
            lastTargetPositions[i] = lastTargetPositions[i - 1];
        lastTargetPositions[0] = target.position;

        for (int i = lastTargetDirections.Length - 1; i > 0; i--)
            lastTargetDirections[i] = lastTargetDirections[i];
        lastTargetDirections[0] = target.forward;
    }

}
