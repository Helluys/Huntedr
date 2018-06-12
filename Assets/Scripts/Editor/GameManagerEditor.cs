using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor {
    
    public override void OnInspectorGUI () {
        base.OnInspectorGUI();

        GameManager gameManager = target as GameManager;
        if (GUILayout.Button("Instantiate win conditions")) {
            gameManager.winConditions = Instantiate(gameManager.winConditions);
        }
    }
}