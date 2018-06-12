using UnityEngine;
using UnityEngine.UI;

public class StatusPanelView : MonoBehaviour {

    [SerializeField] private Ship ship;
    [SerializeField] Text healthField;
    [SerializeField] Text energyField;
    [SerializeField] Text ammunitionField;

    void Start () {
        ship.status.OnHealthChanged += UpdateHealth;
        ship.status.OnEnergyChanged += UpdateEnergy;
        ship.status.OnAmmunitionChanged += UpdateAmmunition;
    }

    private void UpdateHealth (object sender, float healthDelta) {
        healthField.text = ship.status.GetHealth().ToString();
    }
    private void UpdateEnergy(object sender, float energyDelta) {
        energyField.text = ship.status.GetEnergy().ToString();
    }
    private void UpdateAmmunition (object sender, float ammunitionDelta) {
        ammunitionField.text = ship.status.GetAmmunition().ToString();
    }
}
