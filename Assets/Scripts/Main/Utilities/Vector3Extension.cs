﻿using UnityEngine;

public static class Vector3Extension {

    public static float Mahalanobis (this Vector3 vector) {
        return vector.x + vector.y + vector.z;
    }

    public static Vector3 RandomRange (this Vector3 vector, float min, float max) {
        return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
    }

    public static Vector3 Sign (this Vector3 vector) {
        return new Vector3(Mathf.Sign(vector.x), Mathf.Sign(vector.y), Mathf.Sign(vector.z));
    }

}
