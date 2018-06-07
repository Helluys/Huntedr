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

        public static Vector3 GetRandomColliderPoint(Collider collider) {
            Vector3 point;

            if (collider is BoxCollider)
                point = GetRandomBoxColliderPoint(collider as BoxCollider);
            else if (collider is SphereCollider)
                point = GetRandomSphereColliderPoint(collider as SphereCollider);
            else if (collider is CapsuleCollider)
                point = GetRandomCapsuleColliderPoint(collider as CapsuleCollider);
            else
                throw new System.NotImplementedException();

            return point;
        }

        public static Vector3 GetRandomCapsuleColliderPoint (CapsuleCollider capsuleCollider) {
            Vector3 point = capsuleCollider.center;

            // convert capsule direction to a vector
            Vector3 direction;
            switch (capsuleCollider.direction) {
                case 0:
                    direction = Vector3.right;
                    break;
                case 1:
                    direction = Vector3.up;
                    break;
                case 2:
                    direction = Vector3.forward;
                    break;
                default:
                    // robustness : should not happen
                    direction = Vector3.zero;
                    break;
            }

            if (capsuleCollider.radius > 0f) {
                // avoid a negative value of height
                float cylinderHeight = Mathf.Max(0f, capsuleCollider.height - 2f * capsuleCollider.radius);
                float cylinderOverSphereVolumeRatio = (3f / 4f) * cylinderHeight / capsuleCollider.radius;

                // if random value is greater than the volume ratio, randomize spawn point in cylinder
                if (Random.value < cylinderOverSphereVolumeRatio / (1 + cylinderOverSphereVolumeRatio)) {

                    Vector2 randomCircle = Random.insideUnitCircle;
                    switch (capsuleCollider.direction) {
                        case 0:
                            point += new Vector3(0f, randomCircle.x, randomCircle.y);
                            break;
                        case 1:
                            point += new Vector3(randomCircle.x, 0f, randomCircle.y);
                            break;
                        case 2:
                            point += new Vector3(randomCircle.x, randomCircle.y, 0f);
                            break;
                    }

                    point += (Random.value - 0.5f) * cylinderHeight * direction;

                } else { // randomize spawn point in one of the two hemisphere
                    Vector3 randomSpherePoint = Random.insideUnitSphere * capsuleCollider.radius;
                    point += direction * (Vector3.Dot(randomSpherePoint, direction) > 0f ? 1f : -1f) * cylinderHeight / 2f + randomSpherePoint;
                }

            } else {
                // a null radius means capsule is a line or a point if null height
                point += (Random.value - 0.5f) * capsuleCollider.height * direction;
            }

            return point;
        }

        public static Vector3 GetRandomSphereColliderPoint (SphereCollider sphereCollider) {
            return sphereCollider.center +
                Random.insideUnitSphere * Random.value * sphereCollider.radius;
        }

        public static Vector3 GetRandomBoxColliderPoint (BoxCollider boxCollider) {
            return boxCollider.center +
                new Vector3((Random.value - 0.5f) * boxCollider.size.x,
                            (Random.value - 0.5f) * boxCollider.size.y,
                            (Random.value - 0.5f) * boxCollider.size.z);
        }
    }

}
