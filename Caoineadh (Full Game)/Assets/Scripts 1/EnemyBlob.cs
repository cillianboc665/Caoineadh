using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlob : MonoBehaviour
{
    public float scale = 1.0f;
    public float speed = 1.0f;
    public Transform leftEye;
    public Transform rightEye;

    [Header("Vertex Selection")]
    public int leftEyeVertexIndex = 0;
    public int rightEyeVertexIndex = 1;
    public bool showVertexGizmos = true;

    private bool recalculateNormals = false;
    private Vector3[] baseVertices;
    private Vector3[] vertices;
    private Mesh mesh;

    public Transform player;
    public bool eyesShouldLookAtPlayer = false;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        baseVertices = mesh.vertices;
        vertices = new Vector3[baseVertices.Length];

        // Try to find reasonable starting vertices
        if (leftEyeVertexIndex >= baseVertices.Length) leftEyeVertexIndex = 0;
        if (rightEyeVertexIndex >= baseVertices.Length) rightEyeVertexIndex = 1;
    }

    void Update()
    {
        CalcNoise();
        UpdateEyePositions();
    }

    void CalcNoise()
    {
        if (baseVertices == null) return;

        float timex = Time.time * speed + 2.5564f;
        float timey = Time.time * speed + 1.21688f;
        float timez = Time.time * speed + 0.1365143f;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];
            vertex.x += Mathf.PerlinNoise(timex + vertex.x, timex + vertex.y) * scale;
            vertex.y += Mathf.PerlinNoise(timey + vertex.x, timey + vertex.y) * scale;
            vertex.z += Mathf.PerlinNoise(timez + vertex.x, timez + vertex.y) * scale;
            vertices[i] = vertex;
        }

        mesh.vertices = vertices;
        if (recalculateNormals)
        {
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }

    void UpdateEyePositions()
    {
        if (vertices != null && vertices.Length > 0)
        {
            Vector3 leftPos = transform.TransformPoint(vertices[leftEyeVertexIndex]);
            Vector3 rightPos = transform.TransformPoint(vertices[rightEyeVertexIndex]);

            leftEye.position = leftPos;
            rightEye.position = rightPos;
        }

        if (eyesShouldLookAtPlayer && player != null)
        {
            Quaternion leftRot = Quaternion.LookRotation(player.position - leftEye.position);
            Quaternion rightRot = Quaternion.LookRotation(player.position - rightEye.position);

            leftEye.rotation = leftRot;
            rightEye.rotation = rightRot;
        }
        else
        {
            leftEye.rotation = Quaternion.LookRotation(transform.forward);
            rightEye.rotation = Quaternion.LookRotation(transform.forward);
        }
    }

    // Debug visualization - THIS SHOWS THE ACTUAL VERTICES BEING USED
    void OnDrawGizmosSelected()
    {
        if (!showVertexGizmos || mesh == null) return;

        // Use current vertices if available, otherwise use base vertices
        Vector3[] currentVertices = (vertices != null && vertices.Length > 0) ? vertices : mesh.vertices;

        // Draw left eye vertex
        if (leftEyeVertexIndex < currentVertices.Length && leftEyeVertexIndex >= 0)
        {
            Gizmos.color = Color.red;
            Vector3 worldPos = transform.TransformPoint(currentVertices[leftEyeVertexIndex]);
            Gizmos.DrawSphere(worldPos, 0.1f);
            Gizmos.DrawWireSphere(worldPos, 0.15f);
        }

        // Draw right eye vertex
        if (rightEyeVertexIndex < currentVertices.Length && rightEyeVertexIndex >= 0)
        {
            Gizmos.color = Color.blue;
            Vector3 worldPos = transform.TransformPoint(currentVertices[rightEyeVertexIndex]);
            Gizmos.DrawSphere(worldPos, 0.1f);
            Gizmos.DrawWireSphere(worldPos, 0.15f);
        }

        // Draw all vertices as small dots to help with selection
        Gizmos.color = Color.green;
        for (int i = 0; i < currentVertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(currentVertices[i]);
            Gizmos.DrawSphere(worldPos, 0.02f);
        }
    }

    // NEW: Helper method to find closest vertex to a world position
    public int FindClosestVertex(Vector3 worldPosition)
    {
        if (vertices == null || vertices.Length == 0) return -1;

        float closestDistance = float.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertexWorldPos = transform.TransformPoint(vertices[i]);
            float distance = Vector3.Distance(vertexWorldPos, worldPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    // NEW: Auto-find eye vertices based on mesh bounds
    [ContextMenu("Auto-Find Eye Vertices")]
    public void AutoFindEyeVertices()
    {
        if (mesh == null || vertices == null) return;

        // Find vertices at the "front-top" of the mesh
        List<int> candidateVertices = new List<int>();
        Bounds bounds = mesh.bounds;

        for (int i = 0; i < vertices.Length; i++)
        {
            // Look for vertices in the top-front quadrant
            if (vertices[i].y > bounds.center.y && vertices[i].z > bounds.center.z)
            {
                candidateVertices.Add(i);
            }
        }

        if (candidateVertices.Count >= 2)
        {
            // Sort by X position to find left and right
            candidateVertices.Sort((a, b) => vertices[a].x.CompareTo(vertices[b].x));

            leftEyeVertexIndex = candidateVertices[0]; // Left-most
            rightEyeVertexIndex = candidateVertices[candidateVertices.Count - 1]; // Right-most

            Debug.Log($"Auto-found vertices: Left={leftEyeVertexIndex}, Right={rightEyeVertexIndex}");
        }
    }

    // NEW: Print current vertex positions for debugging
    [ContextMenu("Debug Vertex Positions")]
    public void DebugVertexPositions()
    {
        if (vertices == null) return;

        Debug.Log($"Left eye vertex {leftEyeVertexIndex}: {vertices[leftEyeVertexIndex]}");
        Debug.Log($"Right eye vertex {rightEyeVertexIndex}: {vertices[rightEyeVertexIndex]}");
    }
}