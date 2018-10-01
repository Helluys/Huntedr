using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/AI Ship Controller", fileName = "AIController")]
public class AIControllerModel : ShipControllerModel {


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

        public float desiredDistance { get; set; } = 30f;

        private LowLevelObjective currentObjective;

        public Vector3 targetPosition { get { return this.controlComputer.target.point; } }
        public Vector3 targetAim { get { return this.controlComputer.target.aim; } }

        private readonly Ship ship;
        private readonly AIControllerModel model;

        private float updateTime = 0f;

        private readonly SlidingModeControl controlComputer;
        private AISubController currentSubController;

        public AIControllerInstance (Ship ship, AIControllerModel model) {
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
            // Nothing to do
        }

        public override void OnUpdate () {
            if (IsUpdateFrame() && this.currentSubController != null) {
                this.controlComputer.target = this.currentSubController.ComputeTarget();

                ShipEngine.Input engineInput = this.controlComputer.ComputeControl();
                ApplyInaccuracies(engineInput);

                this.ship.engine.input = engineInput;
            }
        }

        public void SetObjective (LowLevelObjective objective) {
            this.currentObjective = objective;

            switch (this.currentObjective?.type) {
                case LowLevelObjective.Type.TargetShip:
                    this.currentSubController = new ShipTargeter(this.ship, objective.target.GetComponent<Ship>(), this.desiredDistance, this.model.anticipationFactor);
                    break;
                case LowLevelObjective.Type.TargetObject:
                    this.currentSubController = new ObjectTargeter(this.ship, objective.target.GetComponent<IDestructible>(), this.desiredDistance);
                    break;
                case LowLevelObjective.Type.MoveToPoint:
                    this.currentSubController = new PathFollower(this.ship, objective.point);
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
