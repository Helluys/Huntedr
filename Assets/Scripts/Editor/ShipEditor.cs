using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(Ship))]
public class ShipEditor : Editor {

    public override void OnInspectorGUI () {
        base.OnInspectorGUI();

        Ship ship = target as Ship;
        if (GUILayout.Button("Reset with model")) {
            ship.ResetModels();
        }
    }

}