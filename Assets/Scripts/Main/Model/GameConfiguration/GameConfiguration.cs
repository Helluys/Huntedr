using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Game configuration", menuName = "Game data/Game configuration/Game configuration")]
public class GameConfiguration : ScriptableObject {

    public GlobalGameData.MapData map;
    public List<TeamConfiguration> teams = new List<TeamConfiguration>();

    public AsyncOperation LoadGame () {
        DontDestroyOnLoad(this);

        Resources.Load(GlobalGameData.MAP_FOLDER + "map.sceneName");
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(map.sceneName);
        asyncOperation.completed += SetupGame;
        return asyncOperation;
    }

    private void SetupGame (AsyncOperation sceneLoadOperation) {
        GameObject.Find("GameManager").GetComponent<GameManager>().gameConfiguration = this;
    }
}
