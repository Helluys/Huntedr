using UnityEngine;
using UnityEngine.UI;

public class StatusPanelView : MonoBehaviour {

    [SerializeField] private Ship ship;
    [SerializeField] Text healthField;
    [SerializeField] Text energyField;
    [SerializeField] Text ammunitionField;

    void Start () {
        if (GameManager.instance.playerList.Count == 0)
            gameObject.SetActive(false);
        else {
            ship = GameManager.instance.playerList[0];
            ship.status.OnHealthChanged += UpdateHealth;
            ship.status.OnEnergyChanged += UpdateEnergy;
            ship.status.OnAmmunitionChanged += UpdateAmmunition;
        }
    }

    private void UpdateHealth (object sender, float healthDelta) {
        healthField.text = ship.status.GetHealth().ToString("N2");
    }
    private void UpdateEnergy (object sender, float energyDelta) {
        energyField.text = ship.status.GetEnergy().ToString("N2");
    }
    private void UpdateAmmunition (object sender, float ammunitionDelta) {
        ammunitionField.text = ship.status.GetAmmunition().ToString();
    }
}
