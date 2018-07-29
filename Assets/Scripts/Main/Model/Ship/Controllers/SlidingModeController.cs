using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/Sliding mode AI Ship Controller", fileName = "SlidingModeController")]
public class SlidingModeController : ShipControllerModel {

    [SerializeField] private Vector3 thrustFactors;
    [SerializeField] private Vector3 torqueFactors;

    public override Instance CreateInstance (Ship holder) {
        return new SlidingModeControllerInstance(holder, this);
    }

    public class SlidingModeControllerInstance : Instance {

        public Transform target;

        private Ship ship;
        private SlidingModeController model;

        private WaitForFixedUpdate waiter = new WaitForFixedUpdate();

        public struct Pose {
            public Vector3 position;
            public Quaternion attitude;

            public float this[int i] {
                get { return i < 2 ? position[i] : attitude[i - 3]; }
                set {
                    if (i < 2)
                        position[i] = value;
                    else
                        attitude[i - 3] = value;
                }
            }
        }

        public SlidingModeControllerInstance (Ship ship, SlidingModeController model) {
            this.ship = ship;
            this.model = model;
        }

        public override void OnStart () {
            // Nothing to initialize
        }

        public override void OnUpdate () {
            if (target == null)
                return;

            Pose error = ComputeError();

            Vector3 thrust = Vector3.Scale(this.model.thrustFactors, error.position.Sign());

            // Correct attitude error from [0; 360] to [-180; 180]
            Vector3 correctedAttitudeError = error.attitude.eulerAngles;
            for (int i = 0; i < 3; i++)
                correctedAttitudeError[i] -= (correctedAttitudeError[i] > 180f) ? 360f : 0f;

            Vector3 torque = Vector3.Scale(this.model.torqueFactors, correctedAttitudeError.Sign());

            ship.engine.inputThrust = thrust;
            ship.engine.inputTorque = torque;
        }

        private Pose ComputeError () {
            return new Pose {
                position = this.ship.transform.InverseTransformPoint(this.target.position),
                attitude = Quaternion.Inverse(this.ship.transform.rotation) * Quaternion.LookRotation(this.target.position - this.ship.transform.position)
            };
        }
    }
}
