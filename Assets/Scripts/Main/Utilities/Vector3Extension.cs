using UnityEngine;

public static class Vector3Extension {
    
    public static float Mahalanobis (this Vector3 vector) {
        return vector.x + vector.y + vector.z;
    }

}
