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

}
