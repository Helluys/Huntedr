using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/AI Ship Controller", fileName = "AIController")]
public class AIController : ShipControllerModel {

    [SerializeField] private Vector3 thrustFactors;
    [SerializeField] private Vector3 torqueFactors;

    [SerializeField] private Vector3 thrustDeltas;
    [SerializeField] private Vector3 torqueDeltas;

    [SerializeField] private float anticipationFactor = 0.1f;

    [SerializeField] [Range(0f, 1f)] public float accuracy = 0.8f;

    public override Instance CreateInstance (Ship holder) {
        return new AIControllerInstance(holder, this);
    }

    public class AIControllerInstance : Instance {

        public Transform target {
            get { return this.shipSimulator.target; }
            set { this.shipSimulator.target = value; }
        }

        public float desiredDistance { get; set; } = 30f;

        public Vector3 targetPosition { get { return this.controlComputer.targetPosition; } }
        public Vector3 targetAim { get { return this.controlComputer.targetAim; } }
        private Ship ship;
        private AIController model;

        private ShipSimulator shipSimulator;
        private SlidingModeControl controlComputer;

        public AIControllerInstance (Ship ship, AIController model) {
            this.ship = ship;
            this.model = model;

            this.shipSimulator = new ShipSimulator(null);
            this.controlComputer = new SlidingModeControl(ship.transform);
        }

        public override void OnStart () {
            this.controlComputer.thrustFactors = this.model.thrustFactors;
            this.controlComputer.thrustDeltas = this.model.thrustDeltas;
            this.controlComputer.torqueFactors = this.model.torqueFactors;
            this.controlComputer.torqueDeltas = this.model.torqueDeltas;

            this.ship.StartCoroutine(UpdateInputs());
        }

        public override void OnUpdate () {
            // Nothing to do
        }

        private IEnumerator UpdateInputs () {
            for (; ; ) {
                if (this.target == null)
                    yield return new WaitForSeconds(1f - this.model.accuracy);

                this.shipSimulator.anticipationTime = this.model.anticipationFactor * (this.target.position - this.ship.transform.position).magnitude;
                this.shipSimulator.UpdateSimulator();

                this.controlComputer.targetPosition = this.shipSimulator.simulatedPosition - (this.shipSimulator.simulatedPosition - this.ship.transform.position).normalized * desiredDistance;
                this.controlComputer.targetAim = this.shipSimulator.simulatedPosition;

                SlidingModeControl.EngineInput engineInput = this.controlComputer.ComputeControl();
                ApplyInaccuracies(engineInput);

                this.ship.engine.inputThrust = engineInput.thrust;
                this.ship.engine.inputTorque = engineInput.torque;

                yield return new WaitForSeconds(1f - this.model.accuracy);
            }
        }

        private void ApplyInaccuracies (SlidingModeControl.EngineInput engineInput) {
            engineInput.thrust += Random.insideUnitSphere * (1f - model.accuracy);
            engineInput.torque += Random.insideUnitSphere * (1f - model.accuracy);
        }
    }
}
