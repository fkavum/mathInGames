using UnityEngine;


public class Playground : MonoBehaviour
{
    [Header("Direction to Rotation")]
    [SerializeField] private Transform upObj;
    [SerializeField] private Transform rightObj;
    
    [SerializeField] private Vector3 directionForUp;
    [SerializeField] private Vector3 directionForRight;


    [Header("Rotation to Direction")]
    [SerializeField] private Vector3 rotationForUp;
    [SerializeField] private Vector3 rotationForRight;
    [SerializeField] private Transform rotationUpObj;
    [SerializeField] private Transform rotationRightObj;

    void Update()
    {
        Vector3 defaultDirection = Vector3.up;
        Quaternion rotation = Quaternion.LookRotation(directionForUp, Vector3.up);
        Quaternion correctionRotation = Quaternion.FromToRotation(defaultDirection, Vector3.forward);
        Quaternion finalRotation = rotation * correctionRotation;
        
        Quaternion finalRotation2 = QuaternionExtensions.RotationFromDirection(directionForRight,Vector3.right);

        // Quaternion rotationForUp = Quaternion.LookRotation(directionForUp, Vector3.right);
        // Quaternion resultRotationForRight = Quaternion.LookRotation(directionForRight, Vector3.up);

        upObj.rotation = finalRotation;
        rightObj.rotation = finalRotation2;
        
        /////////////========
        Quaternion rotationValue = rotationRightObj.rotation;
        rotationForRight = QuaternionExtensions.DirectionFromRotation(rotationValue, Vector3.right);
        rotationForUp = QuaternionExtensions.DirectionFromRotation(rotationUpObj.rotation, Vector3.up);
    }
}

public static class QuaternionExtensions
{
    public static Quaternion LookRotation(Vector3 forward, Vector3 up)
    {
        forward = forward.normalized; // Ensure the forward vector is normalized
        Vector3 right = Vector3.Cross(up, forward).normalized; // Calculate the right vector
        up = Vector3.Cross(forward, right); // Recalculate the true up vector

        // Construct the rotation matrix
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetColumn(0, new Vector4(right.x, right.y, right.z, 0));
        matrix.SetColumn(1, new Vector4(up.x, up.y, up.z, 0));
        matrix.SetColumn(2, new Vector4(forward.x, forward.y, forward.z, 0));
        matrix.SetColumn(3, new Vector4(0, 0, 0, 1));

        return QuaternionFromMatrix(matrix);
    }

    private static Quaternion QuaternionFromMatrix(Matrix4x4 matrix)
    {
        Quaternion quaternion = new Quaternion();
        float trace = matrix.m00 + matrix.m11 + matrix.m22;

        if (trace > 0f)
        {
            float s = Mathf.Sqrt(trace + 1f) * 2f;
            quaternion.w = 0.25f * s;
            quaternion.x = (matrix.m21 - matrix.m12) / s;
            quaternion.y = (matrix.m02 - matrix.m20) / s;
            quaternion.z = (matrix.m10 - matrix.m01) / s;
        }
        else if ((matrix.m00 > matrix.m11) && (matrix.m00 > matrix.m22))
        {
            float s = Mathf.Sqrt(1f + matrix.m00 - matrix.m11 - matrix.m22) * 2f;
            quaternion.w = (matrix.m21 - matrix.m12) / s;
            quaternion.x = 0.25f * s;
            quaternion.y = (matrix.m01 + matrix.m10) / s;
            quaternion.z = (matrix.m02 + matrix.m20) / s;
        }
        else if (matrix.m11 > matrix.m22)
        {
            float s = Mathf.Sqrt(1f + matrix.m11 - matrix.m00 - matrix.m22) * 2f;
            quaternion.w = (matrix.m02 - matrix.m20) / s;
            quaternion.x = (matrix.m01 + matrix.m10) / s;
            quaternion.y = 0.25f * s;
            quaternion.z = (matrix.m12 + matrix.m21) / s;
        }
        else
        {
            float s = Mathf.Sqrt(1f + matrix.m22 - matrix.m00 - matrix.m11) * 2f;
            quaternion.w = (matrix.m10 - matrix.m01) / s;
            quaternion.x = (matrix.m02 + matrix.m20) / s;
            quaternion.y = (matrix.m12 + matrix.m21) / s;
            quaternion.z = 0.25f * s;
        }

        return quaternion.normalized;
    }
    
    
    public static Quaternion FromToRotation(Vector3 from, Vector3 to)
    {
        from = from.normalized; // Normalize the 'from' vector
        to = to.normalized;     // Normalize the 'to' vector

        float dot = Vector3.Dot(from, to); // Compute the dot product
        if (dot > 0.9999f) // Vectors are nearly parallel
        {
            return Quaternion.identity; // No rotation needed
        }
        else if (dot < -0.9999f) // Vectors are nearly opposite
        {
            // Find an orthogonal vector to construct a 180-degree rotation
            Vector3 orthogonal = Vector3.Cross(from, Vector3.right);
            if (orthogonal.magnitude < 0.001f) // If parallel to right vector, use another vector
            {
                orthogonal = Vector3.Cross(from, Vector3.up);
            }
            orthogonal.Normalize();
            return Quaternion.AngleAxis(180f, orthogonal);
        }

        Vector3 cross = Vector3.Cross(from, to); // Calculate the cross product
        float s = Mathf.Sqrt((1f + dot) * 2f);   // Calculate the scale factor
        float invS = 1f / s;

        return new Quaternion(
            cross.x * invS,
            cross.y * invS,
            cross.z * invS,
            s * 0.5f
        ).normalized; // Normalize the quaternion
    }
    
    public static Quaternion RotationFromDirection(Vector3 direction, Vector3 defaultDir)
    {
        Quaternion rotation2 = LookRotation(direction, Vector3.up); // Calculate rotation to make the arrow point in the specified direction
        Quaternion correctionRotation2 = FromToRotation(defaultDir, Vector3.forward); // Calculate the alignment correction rotation
        Quaternion finalRotation2 = rotation2 * correctionRotation2;
        return finalRotation2;
    }
    
    public static Vector3 DirectionFromRotation(Quaternion rotation, Vector3 defaultDirection)
    {
        // Calculate the correction rotation as the inverse of FromToRotation
        Quaternion correctionRotation = FromToRotation(defaultDirection, Vector3.forward);
        correctionRotation = Quaternion.Inverse(correctionRotation);
        
        // Apply the inverse correction to the rotation to retrieve the direction
        Quaternion inverseRotation = rotation * correctionRotation;
        
        // Transform the forward vector (or default direction) by the inverse rotation to get the original direction
        return inverseRotation * Vector3.forward;
    }
}