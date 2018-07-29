using System.Collections;

using UnityEngine;
using Utilities;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/AI Ship Controller", fileName = "AIShipController")]
public class AIShipController : ShipControllerModel {

    [SerializeField] private float a;
    [SerializeField] private float b;
    [SerializeField] private float ra;
    [SerializeField] private float rb;

    public override Instance CreateInstance (Ship holder) {
        return new AIShipControllerInstance(holder, this);
    }

    public class AIShipControllerInstance : Instance {

        public Transform target;

        private Vector3[] lastTargetPositions = new Vector3[3];
        private Vector3[] lastTargetDirections = new Vector3[3];

        public Vector3 targetVelocity;
        public Vector3 targetAngularVelocity;

        public Vector3 targetAcceleration;
        public Vector3 targetAngularAcceleration;

        public float anticipationFactor = 0.1f;
        private float anticipationTime;

        public Vector3 targetPoint;
        public Vector3 targetDirection;

        private Ship ship;
        private AIShipController model;
        private WaitForSeconds waitOneSecond = new WaitForSeconds(1f);
        private const int SIMULATION_POOL = 30;

        private WaitForFixedUpdate waiter = new WaitForFixedUpdate();

        public AIShipControllerInstance (Ship ship, AIShipController model) {
            this.ship = ship;
            this.model = model;
        }

        public override void OnStart () {
            ship.StartCoroutine(UpdateInput());
        }

        public override void OnUpdate () {

        }

        private IEnumerator UpdateInput () {
            yield return null;
            lastTargetPositions = new Vector3[SIMULATION_POOL];
            lastTargetDirections = new Vector3[SIMULATION_POOL];
            for (int i = 0; i < lastTargetDirections.Length; i++) {
                lastTargetPositions[i] = Vector3.zero;
                lastTargetDirections[i] = Vector3.zero;
            }

            for (; ; ) {
                if (target != null) {
                    UpdateSimulator();

                    Vector3 localTargetPoint = ship.transform.InverseTransformPoint(targetPoint);
                    ship.engine.inputThrust = model.a * localTargetPoint;

                    Vector3 targetDirection = (targetPoint - ship.transform.position).normalized;
                    Vector3 anglesDelta = Vector3.SignedAngle(ship.transform.forward, Vector3.ProjectOnPlane(targetDirection, ship.transform.right), ship.transform.right) * Vector3.right
                                        + Vector3.SignedAngle(ship.transform.forward, Vector3.ProjectOnPlane(targetDirection, ship.transform.up), ship.transform.up) * Vector3.up;
                    ship.engine.inputTorque = model.ra * anglesDelta / 90f;
                }

                yield return waiter;
            }
        }

        private void UpdateSimulator () {
            ShiftTargetArrays();

            Vector3[] velocityArray = MathUtils.ArrayDelta(lastTargetPositions, 1f / Time.fixedDeltaTime);
            targetVelocity = MathUtils.ArrayAverage(velocityArray);

            Vector3[] angularVelocityArray = MathUtils.ArrayDelta(lastTargetDirections, 1f / Time.fixedDeltaTime);
            targetAngularVelocity = MathUtils.ArrayAverage(angularVelocityArray);

            targetPoint = target.position;
            targetDirection = target.forward;

            this.anticipationTime = (this.target.position - this.ship.transform.position).magnitude * this.anticipationFactor;
            float simulationTime = 0f;
            while (simulationTime < anticipationTime) {
                this.targetPoint += this.targetVelocity * Time.fixedDeltaTime;
                this.targetDirection += this.targetAngularVelocity * Time.fixedDeltaTime;

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
}
