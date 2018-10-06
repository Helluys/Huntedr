using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtils {

    public static void SetColorRecursive (Transform target, Color color) {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = color;

        foreach (Transform child in target)
            SetColorRecursive(child, color);
    }

    public static void SetColorRecursive (Transform target, Color primaryColor, Color secondaryColor) {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = primaryColor;

        foreach (Transform child in target)
            SetColorRecursive(child, secondaryColor, primaryColor);
    }

    public static GameObject GetClosest (GameObject gameObject, List<GameObject> others) {
        float minDistance = Mathf.Infinity;
        GameObject closest = null;

        foreach (GameObject go in others) {
            float distance = (gameObject.transform.position - go.transform.position).magnitude;

            if (distance < minDistance)
                closest = go;
        }

        return closest;
    }

    public static bool CanSee (Ship ship, GameObject target) {
        RaycastHit hitInfo;
        return Physics.Raycast(ship.transform.position, (target.transform.position - ship.transform.position).normalized, out hitInfo, Mathf.Infinity) && hitInfo.transform.gameObject.Equals(target);
    }

}
