using UnityEngine;
using UnityEngine.UI;

public class StatusPanelView : MonoBehaviour {

    [SerializeField] private Ship ship;
    [SerializeField] Text healthField;
    [SerializeField] Text energyField;
    [SerializeField] Text ammunitionField;

    void Start () {
        ship.shipStatus.OnHealthChanged += UpdateHealth;
        ship.shipStatus.OnEnergyCHanged += UpdateEnergy;
        ship.shipStatus.OnAmmunitionChanged += UpdateAmmunition;
    }

    private void UpdateHealth (object sender, float newHealth) {
        healthField.text = newHealth.ToString();
    }
    private void UpdateEnergy(object sender, float newEnergy) {
        energyField.text = newEnergy.ToString();
    }
    private void UpdateAmmunition (object sender, float newAmmunition) {
        ammunitionField.text = newAmmunition.ToString();
    }
}
