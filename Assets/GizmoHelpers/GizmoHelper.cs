using UnityEngine;

public static class GizmoHelper
{
    /// <summary>
    /// Draws an arrow with the specified parameters, using a direction vector.
    /// </summary>
    public static void DrawArrow(Vector3 startPosition, Vector3 direction, float magnitude, Color color, float thickness = 0.1f)
    {
        Gizmos.color = color;

        Vector3 endPosition = startPosition + direction.normalized * magnitude;
        DrawArrowBody(startPosition, endPosition, direction, magnitude, thickness);
    }

    /// <summary>
    /// Draws an arrow with the specified parameters, using a rotation in Quaternion.
    /// </summary>
    public static void DrawArrow(Vector3 startPosition, Quaternion rotation, float magnitude, Color color, float thickness = 0.1f)
    {
        Vector3 direction = rotation * Vector3.forward;
        DrawArrow(startPosition, direction, magnitude, color, thickness);
    }

    /// <summary>
    /// Draws an arrow with the specified parameters, using Euler angles for rotation.
    /// </summary>
    public static void DrawArrow(Vector3 startPosition, Vector3 eulerRotation, float magnitude, Color color,string name, float thickness = 0.1f)
    {
        Quaternion rotation = Quaternion.Euler(eulerRotation);
        DrawArrow(startPosition, rotation, magnitude, color, thickness);
    }



    /// <summary>
    /// Helper method to draw the arrow body and 3D arrowhead.
    /// </summary>
    private static void DrawArrowBody(Vector3 startPosition, Vector3 endPosition, Vector3 direction, float magnitude, float thickness)
    {
        // Draw the main line of the arrow
        Gizmos.DrawLine(startPosition, endPosition);

        // Draw 3D arrowhead as a cone
        float arrowHeadLength = magnitude * 0.2f;
        float arrowHeadRadius = Mathf.Sqrt(magnitude) * 0.2f; //*thickness

        Draw3DArrowhead(endPosition, direction, arrowHeadLength, arrowHeadRadius);

        // Draw thickness for the arrow shaft
        if (thickness > 0.01f)
        {
            Vector3 perpendicularOffset = Vector3.Cross(direction, Vector3.up).normalized * thickness * 0.5f;
            Gizmos.DrawLine(startPosition - perpendicularOffset, endPosition - perpendicularOffset);
            Gizmos.DrawLine(startPosition + perpendicularOffset, endPosition + perpendicularOffset);
        }
    }

    /// <summary>
    /// Draws a 3D cone-shaped arrowhead at the end of the arrow.
    /// </summary>
    private static void Draw3DArrowhead(Vector3 position, Vector3 direction, float length, float radius)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        
        // Number of segments around the cone
        int segments = 12;
        Vector3[] baseCirclePoints = new Vector3[segments];
        
        // Calculate the points around the base of the cone
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            Vector3 point = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            baseCirclePoints[i] = position + rotation * (point - Vector3.forward * length);
        }

        // Draw the cone sides
        for (int i = 0; i < segments; i++)
        {
            int nextIndex = (i + 1) % segments;

            // Draw line from tip to each point on the base circle
            Gizmos.DrawLine(position, baseCirclePoints[i]);

            // Draw line between consecutive points around the base circle
            Gizmos.DrawLine(baseCirclePoints[i], baseCirclePoints[nextIndex]);
        }
    }

    /// <summary>
    /// Helper method to draw the arrow body and arrowhead.
    /// </summary>
    private static void DrawArrowBody2(Vector3 startPosition, Vector3 endPosition, Vector3 direction, float magnitude, float thickness)
    {
        // Draw the main line of the arrow
        Gizmos.DrawLine(startPosition, endPosition);

        // Draw arrowhead
        float arrowHeadLength = magnitude * 0.5f;
        float arrowHeadAngle = 20.0f;

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;

        Gizmos.DrawLine(endPosition, endPosition + right * arrowHeadLength);
        Gizmos.DrawLine(endPosition, endPosition + left * arrowHeadLength);

        

        // Draw thickness by drawing additional parallel lines
        if (thickness > 0.01f)
        {
            Vector3 perpendicularOffset = Vector3.Cross(direction, Vector3.up).normalized * thickness * 0.5f * magnitude;
            Gizmos.DrawLine(startPosition - perpendicularOffset, endPosition - perpendicularOffset);
            Gizmos.DrawLine(startPosition + perpendicularOffset, endPosition + perpendicularOffset);

            Gizmos.DrawLine(startPosition - perpendicularOffset/2, endPosition - perpendicularOffset/2);
            Gizmos.DrawLine(startPosition + perpendicularOffset/2, endPosition + perpendicularOffset/2);

            Gizmos.DrawLine(startPosition - perpendicularOffset/4, endPosition - perpendicularOffset/4);
            Gizmos.DrawLine(startPosition + perpendicularOffset/4, endPosition + perpendicularOffset/4);
        }
    }
}

    /// <summary>
    /// Draws an arrow with the specified parameters.
    /// </summary>
    /// <param name="startPosition">The starting position of the arrow.</param>
    /// <param name="direction">The direction of the arrow.</param>
    /// <param name="magnitude">The length of the arrow.</param>
    /// <param name="color">The color of the arrow.</param>
    /// <param name="thickness">The thickness of the arrow line.</param>
    // public static void DrawArrow(Vector3 startPosition, Vector3 direction, float magnitude, Color color, float thickness = 0.1f)
    // {
    //     Gizmos.color = color;

    //     // Calculate the end position of the arrow
    //     Vector3 endPosition = startPosition + direction.normalized * magnitude;

    //     // Draw the main line of the arrow
    //     Gizmos.DrawLine(startPosition, endPosition);

    //     // Draw arrowhead with a smaller magnitude and an angle
    //     float arrowHeadLength = magnitude * 0.2f;
    //     float arrowHeadAngle = 20.0f;

    //     // Calculate the two directions for the arrowhead
    //     Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
    //     Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;

    //     // Draw the arrowhead lines
    //     Gizmos.DrawLine(endPosition, endPosition + right * arrowHeadLength);
    //     Gizmos.DrawLine(endPosition, endPosition + left * arrowHeadLength);

    //     // Draw thickness by drawing additional parallel lines
    //     if (thickness > 0.01f)
    //     {
    //         Vector3 perpendicularOffset = Vector3.Cross(direction, Vector3.up).normalized * thickness * 0.5f;
    //         Gizmos.DrawLine(startPosition - perpendicularOffset, endPosition - perpendicularOffset);
    //         Gizmos.DrawLine(startPosition + perpendicularOffset, endPosition + perpendicularOffset);
    //     }
    // }
