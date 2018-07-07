using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamConfigurationWidget : MonoBehaviour {

    [SerializeField] GlobalGameData globalGameData;

    public TeamConfiguration teamConfiguration;
    private int teamIndex;

    [SerializeField] private GameObject shipConfigurationWidgetPrefab;

    #region UI elements

    [SerializeField] private Text label;
    [SerializeField] private InputField nameField;
    [SerializeField] private Dropdown factionDropdown;
    [SerializeField] private RectTransform shipListTransform;

    #endregion

    public void SetUp (GameConfiguration gameConfiguration, int index) {
        teamConfiguration = gameConfiguration.teams[index];
        teamIndex = index;

        Refresh();
    }

    private void Refresh () {
        label.text = "Team " + (teamIndex+1);
        nameField.text = teamConfiguration.name;

        factionDropdown.ClearOptions();
        List<string> factionNames = globalGameData.factions.ConvertAll(faction => faction.name);
        factionDropdown.AddOptions(factionNames);
        factionDropdown.value = factionNames.FindIndex(factionName => factionName.Equals(teamConfiguration.faction.name));

        SetShipCount(teamConfiguration.ships.Count);
    }

    private void SetShipCount (int count) {
        if (count < teamConfiguration.ships.Count)
            teamConfiguration.ships.RemoveRange(count, teamConfiguration.ships.Count - count);
        else if (count > teamConfiguration.ships.Count)
            teamConfiguration.ships.AddRange(globalGameData.GetDefaultShipConfigurationRange(teamConfiguration.ships.Count, count - 1));

        foreach (Transform child in shipListTransform)
            Destroy(child.gameObject);

        for (int i = 0; i < count; i++)
            Instantiate(this.shipConfigurationWidgetPrefab, this.shipListTransform).GetComponent<ShipConfigurationWidget>().SetUp(this.teamConfiguration, i);

        LayoutRebuilder.ForceRebuildLayoutImmediate(GameObject.Find("Canvas").GetComponent<RectTransform>());
    }

    #region UI callbacks

    public void SetTeamName(string teamName) {
        teamConfiguration.name = teamName;
    }

    public void SetFaction (int factionIndex) {
        teamConfiguration.faction = globalGameData.factions[factionIndex];
    }

    public void AddShip() {
        SetShipCount(teamConfiguration.ships.Count + 1);
    }

    #endregion
}
