using UnityEngine;

namespace Racer
{
    public static class MeshGizmoHelper
    {
        public static Mesh defaultArrowMesh;

        /// <summary>
        /// Draws a default 3D arrow mesh as a gizmo at the specified position, rotation, and scale.
        /// </summary>
        public static void DrawMeshArrow(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
        {
            if (defaultArrowMesh == null)
                defaultArrowMesh = CreateArrowMesh();

            DrawCustomMesh(defaultArrowMesh, position, rotation, scale, color, Vector3.up);
        }
        
        public static void DrawMeshArrow(Vector3 position, Vector3 direction, Vector3 scale, Color color)
        {
            if (defaultArrowMesh == null)
                defaultArrowMesh = CreateArrowMesh();

            // Calculate rotation to make the arrow point in the specified direction
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

            // Draw with the calculated rotation
            DrawCustomMesh(defaultArrowMesh, position, rotation, scale, color, Vector3.up);
        }

        /// <summary>
        /// Draws a custom mesh as a gizmo with the specified position, rotation, scale, color, and default direction.
        /// </summary>
        /// <param name="mesh">The custom mesh to be drawn.</param>
        /// <param name="position">The position to place the mesh.</param>
        /// <param name="rotation">The rotation to apply to the mesh.</param>
        /// <param name="scale">The scale of the mesh.</param>
        /// <param name="color">The color to apply to the gizmo.</param>
        /// <param name="defaultDirection">The initial forward direction of the mesh (e.g., Vector3.up, Vector3.forward).</param>
        public static void DrawCustomMesh(Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale, Color color,
            Vector3 defaultDirection)
        {
            Gizmos.color = color;

            // Calculate the alignment correction rotation
            Quaternion correctionRotation = Quaternion.FromToRotation(defaultDirection, Vector3.forward);
            Quaternion finalRotation = rotation * correctionRotation;

            // Set transformation matrix for position, rotation, and scale
            Matrix4x4 matrix = Matrix4x4.TRS(position, finalRotation, scale);
            Gizmos.matrix = matrix;

            Gizmos.DrawMesh(mesh);
        }
        
        public static void DrawCustomMesh(Mesh mesh, Vector3 position, Vector3 direction, Vector3 scale, Color color,
            Vector3 defaultDirection)
        {
            Gizmos.color = color;

            // Calculate rotation to make the arrow point in the specified direction
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            
            // Calculate the alignment correction rotation
            Quaternion correctionRotation = Quaternion.FromToRotation(defaultDirection, Vector3.forward);
            Quaternion finalRotation = rotation * correctionRotation;

            // Set transformation matrix for position, rotation, and scale
            Matrix4x4 matrix = Matrix4x4.TRS(position, finalRotation, scale);
            Gizmos.matrix = matrix;

            Gizmos.DrawMesh(mesh);
        }
        
        
        /// <summary>
        /// Creates a default 3D arrow mesh pointing along the up-axis.
        /// </summary>
        private static Mesh CreateArrowMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = "dcx";
            // Define vertices for the arrow shaft (cylinder) and arrowhead (cone)
            float shaftHeight = 0.7f;
            float shaftRadius = 0.1f;
            float coneHeight = 0.3f;
            float coneRadius = 0.2f;
            int segments = 18;

            // Create the shaft vertices
            Vector3[] vertices = new Vector3[(segments + 1) * 2 + segments + 1];
            int vertIndex = 0;
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * Mathf.PI * 2 / segments;
                float x = Mathf.Cos(angle) * shaftRadius;
                float z = Mathf.Sin(angle) * shaftRadius;

                vertices[vertIndex++] = new Vector3(x, 0, z);
                vertices[vertIndex++] = new Vector3(x, shaftHeight, z);
            }

            // Create the cone vertices
            Vector3 coneTip = new Vector3(0, shaftHeight + coneHeight, 0);
            vertices[vertIndex++] = coneTip; // Cone tip
            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2 / segments;
                float x = Mathf.Cos(angle) * coneRadius;
                float z = Mathf.Sin(angle) * coneRadius;

                vertices[vertIndex++] = new Vector3(x, shaftHeight, z);
            }

            // Define triangles for the shaft and cone
            int[] triangles = new int[segments * 6 + segments * 3];
            int triIndex = 0;

            // Shaft side triangles
            for (int i = 0; i < segments; i++)
            {
                int baseIndex = i * 2;
                triangles[triIndex++] = baseIndex;
                triangles[triIndex++] = baseIndex + 1;
                triangles[triIndex++] = (baseIndex + 2) % ((segments + 1) * 2);

                triangles[triIndex++] = baseIndex + 1;
                triangles[triIndex++] = (baseIndex + 3) % ((segments + 1) * 2);
                triangles[triIndex++] = (baseIndex + 2) % ((segments + 1) * 2);
            }

            // Cone side triangles
            int coneBaseIndex = (segments + 1) * 2;
            for (int i = 0; i < segments; i++)
            {
                triangles[triIndex++] = coneBaseIndex;
                triangles[triIndex++] = coneBaseIndex + i + 1;
                triangles[triIndex++] = coneBaseIndex + (i + 1) % segments + 1;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}