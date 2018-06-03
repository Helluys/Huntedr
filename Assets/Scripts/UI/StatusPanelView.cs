using System;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanelView : MonoBehaviour {

    [SerializeField] private ShipStatus shipStatus;
    [SerializeField] Text healthField;
    [SerializeField] Text energyField;
    [SerializeField] Text ammunitionField;

    void Start () {
        shipStatus.OnHealthChanged += UpdateHealth;
        shipStatus.OnEnergyCHanged += UpdateEnergy;
        shipStatus.OnAmmunitionChanged += UpdateAmmunition;
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
