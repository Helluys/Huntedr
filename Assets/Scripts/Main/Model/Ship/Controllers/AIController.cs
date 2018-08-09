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

        public enum Objective {
            TargetShip, MoveToPoint
        }

        public float desiredDistance { get; set; } = 30f;

        public Objective currentObjective;

        public Vector3 targetPosition { get { return this.controlComputer.target.point; } }
        public Vector3 targetAim { get { return this.controlComputer.target.aim; } }

        private Ship ship;
        private AIController model;

        private float updateTime = 0f;

        private SlidingModeControl controlComputer;
        private AISubController currentSubController;

        public AIControllerInstance (Ship ship, AIController model) {
            this.ship = ship;
            this.model = model;

            this.controlComputer = new SlidingModeControl(ship.transform) {
                thrustFactors = this.model.thrustFactors,
                thrustDeltas = this.model.thrustDeltas,
                torqueFactors = this.model.torqueFactors,
                torqueDeltas = this.model.torqueDeltas
            };
        }

        public override void OnStart () {
        }

        public override void OnUpdate () {
            if (IsUpdateFrame() && currentSubController != null) {
                this.controlComputer.target = currentSubController.ComputeTarget();

                ShipEngine.Input engineInput = this.controlComputer.ComputeControl();
                ApplyInaccuracies(engineInput);

                this.ship.engine.input = engineInput;
            }
        }

        public void SetObjective (Objective objective, Transform target) {
            this.currentObjective = objective;
            switch (this.currentObjective) {
                case Objective.TargetShip:
                    this.currentSubController = new ShipTargeter(this.ship.transform, target, this.desiredDistance, this.model.anticipationFactor);
                    break;
                case Objective.MoveToPoint:
                    this.currentSubController = new PathFollower(this.ship.transform, target.position);
                    break;
            }
        }

        private bool IsUpdateFrame () {
            bool updateFrame = Time.time > this.updateTime;
            if (updateFrame)
                this.updateTime = Time.time + (1f - this.model.accuracy);

            return updateFrame;
        }

        private void ApplyInaccuracies (ShipEngine.Input engineInput) {
            engineInput.thrust += Random.insideUnitSphere * (1f - this.model.accuracy);
            engineInput.torque += Random.insideUnitSphere * (1f - this.model.accuracy);
        }
    }
}
