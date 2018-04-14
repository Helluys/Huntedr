using UnityEngine;

namespace Utilities {

    public struct MathUtils {
        public static Vector3 ClampVector3(Vector3 input, float min, float max) {
            return new Vector3(
                Mathf.Clamp(input.x, min, max),
                Mathf.Clamp(input.y, min, max),
                Mathf.Clamp(input.z, min, max));
        }

        public static Vector3 ClampVector3(Vector3 input, Vector3 min, Vector3 max) {
            return new Vector3(
                Mathf.Clamp(input.x, min.x, max.x),
                Mathf.Clamp(input.y, min.y, max.y),
                Mathf.Clamp(input.z, min.z, max.z));
        }

        public static Matrix4x4 ScaleMatrix(Matrix4x4 m, float f) {
            return new Matrix4x4(
                f * m.GetColumn(0),
                f * m.GetColumn(1),
                f * m.GetColumn(2),
                f * m.GetColumn(3));
        }

    }

}