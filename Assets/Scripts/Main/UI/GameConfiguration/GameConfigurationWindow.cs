using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class GameConfigurationWindow : MonoBehaviour {

    [SerializeField] GlobalGameData globalGameData;

    [SerializeField] private GameConfiguration gameConfiguration;

    [SerializeField] private GameObject teamConfigurationWidgetPrefab;

    #region UI elements

    [SerializeField] private Dropdown mapDropdown;
    [SerializeField] private LayoutGroup teamLayout;
    [SerializeField] private RectTransform loadingImage;

    #endregion

    void Start () {
        gameConfiguration = Instantiate(gameConfiguration);
        loadingImage.gameObject.SetActive(false);
        SetUp();
    }

    private void SetUp () {
        mapDropdown.ClearOptions();

        this.mapDropdown.AddOptions(new List<string>(this.globalGameData.maps.ConvertAll(mapData => mapData.prettyName)));
        SetGameMap(this.mapDropdown.value);
    }

    #region UI callbacks

    public void SetGameMap (int mapIndex) {
        gameConfiguration.map = globalGameData.maps[mapIndex];
        
        SetTeamCount(gameConfiguration.map.teamCount);
    }

    private void SetTeamCount (int count) {
        if (count < gameConfiguration.teams.Count)
            gameConfiguration.teams.RemoveRange(count, gameConfiguration.teams.Count - count);
        else if (count > gameConfiguration.teams.Count)
            gameConfiguration.teams.AddRange(globalGameData.GetDefaultTeamConfigurationRange(gameConfiguration.teams.Count, count - 1));

        foreach (Transform child in teamLayout.transform)
            Destroy(child.gameObject);
        
        for (int i = 0; i < count; i++) {
            GameObject teamConfigurationWidget = Instantiate(this.teamConfigurationWidgetPrefab, this.teamLayout.transform);
            teamConfigurationWidget.GetComponent<TeamConfigurationWidget>().SetUp(this.gameConfiguration, i);
        }
    }

    public void StartGame () {
        StartCoroutine(LoadingGameUI(gameConfiguration.LoadGame()));
    }

    private IEnumerator LoadingGameUI (AsyncOperation asyncOperation) {
        loadingImage.gameObject.SetActive(true);
        while (!asyncOperation.isDone) {
            yield return null;
            loadingImage.Rotate(Vector3.forward, 3f);
        }
    }

    #endregion
}
