using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ShipConfigurationWidget : MonoBehaviour {

    [SerializeField] GlobalGameData globalGameData;

    [SerializeField] private ShipConfiguration shipConfiguration;
    private int shipIndex;

    #region UI elements

    [SerializeField] private Text label;
    [SerializeField] private InputField shipNameField;
    [SerializeField] private Dropdown modelDropdown;
    [SerializeField] private Dropdown controllerDropdown;

    #endregion

    public void SetUp (TeamConfiguration teamConfiguration, int index) {
        shipConfiguration = teamConfiguration.ships[index];
        shipIndex = index;

        Refresh();
    }

    public void Refresh () {
        label.text = "Ship " + (shipIndex+1);
        shipNameField.text = shipConfiguration.name;

        modelDropdown.ClearOptions();
        List<string> modelNames = globalGameData.shipModels.ConvertAll(model => model.name);
        modelDropdown.AddOptions(modelNames);
        modelDropdown.value = modelNames.FindIndex(shipName => shipName.Equals(shipConfiguration.name));

        controllerDropdown.ClearOptions();
        List<string> controllerNames = globalGameData.shipControllers.ConvertAll(controller => controller.name);
        controllerDropdown.AddOptions(controllerNames);
        controllerDropdown.value = controllerNames.FindIndex(controllerName => controllerName.Equals(shipConfiguration.shipControllerModel.name));
    }

    #region UI callbacks

    public void SetShipName(string shipName) {
        shipConfiguration.name = shipName;
    }

    public void SetShipModel(int index) {
        shipConfiguration.shipModel = globalGameData.shipModels[index];
    }

    public void SetShipController(int index) {
        shipConfiguration.shipControllerModel = globalGameData.shipControllers[index];
    }
    
    #endregion
}
