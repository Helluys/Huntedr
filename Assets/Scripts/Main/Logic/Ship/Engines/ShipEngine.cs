using UnityEngine;

[System.Serializable]
public class ShipEngine {

    public ShipStatus shipStatus { get; private set; }

    [SerializeField] private ShipEngineModel shipEngineInstance;

    private Vector3 _inputThrust;
    public Vector3 inputThrust {
        get { return _inputThrust; }
        set { _inputThrust = Vector3.ClampMagnitude(value, 1f); }
    }

    private Vector3 _inputTorque;
    public Vector3 inputTorque {
        get { return _inputTorque; }
        set { _inputTorque = Vector3.ClampMagnitude(value, 1f); }
    }

    private float _inputCushion;
    public float inputCushion {
        get { return _inputCushion; }
        set { _inputCushion = Mathf.Clamp01(value); }
    }

    private float _intputStabilize;
    public float inputStabilize {
        get { return _intputStabilize; }
        set { _intputStabilize = Mathf.Clamp01(value); }
    }

    public ShipEngine (Ship holder) {
        shipStatus = holder.status;

        // Instancing model allows per ship instance model variation
        shipEngineInstance = Object.Instantiate(holder.model.engineModel);
    }

    public virtual void OnUpdate () {
        shipStatus.RefillEnergy(shipEngineInstance.energyProduction * Time.deltaTime);
        ThrottleInput();
    }

    public virtual Vector3 outputThrust {
        get {
            Vector3 thrust = Vector3.zero;
            for (int i = 0; i < 3; i++)
                thrust[i] = shipEngineInstance.flatThrustProfile[i].Evaluate(inputThrust[i]);
            return thrust;
        }
    }

    public virtual Vector3 outputTorque {
        get {
            Vector3 torque = Vector3.zero;
            for (int i = 0; i < 3; i++)
                torque[i] = shipEngineInstance.flatTorqueProfile[i].Evaluate(inputTorque[i]);
            return torque;
        }
    }

    public virtual float outputCushion {
        get { return (Mathf.Atan(inputCushion) / Mathf.PI) * shipEngineInstance.cushionAbility; }
    }

    public virtual float outputStabilize {
        get { return (Mathf.Atan(inputStabilize) / Mathf.PI) * shipEngineInstance.stabilizeAbility; }
    }

    private void ThrottleInput () {

        Vector3 originalThrust = inputThrust;
        Vector3 originalTorque = inputTorque;
        float originalCushion = inputCushion;
        float originalStabilize = inputStabilize;

        for (int i = 9; i > 0 && !this.shipStatus.TryUseEnergy(EnergyConsumption()); i--) {
            inputThrust = 0.1f * i * originalThrust;
            inputTorque = 0.1f * i * originalTorque;
            inputCushion = 0.1f * i * originalCushion;
            inputStabilize = 0.1f * i * originalStabilize;
        }
    }

    private float EnergyConsumption () {
        return (Vector3.Scale(outputThrust, shipEngineInstance.thrustConsumption).Mahalanobis() +
                                    Vector3.Scale(outputTorque, shipEngineInstance.torqueConsumption).Mahalanobis() +
                                    outputCushion * shipEngineInstance.cushionConsumption +
                                    outputStabilize * shipEngineInstance.stabilizeConsumption) * Time.deltaTime;
    }
}
