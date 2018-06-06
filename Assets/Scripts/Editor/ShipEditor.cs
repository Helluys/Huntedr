using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(Ship))]
public class ShipEditor : Editor {

    public override void OnInspectorGUI () {
        base.OnInspectorGUI();

        Ship ship = (Ship) target;
        if (GUILayout.Button("Reset with model")) {
            ship.Start();
        }
    }

}