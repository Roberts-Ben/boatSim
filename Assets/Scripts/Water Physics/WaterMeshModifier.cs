using UnityEngine;

public class WaterMeshModifier : MonoBehaviour
{
    private Mesh waterMesh;
    private Vector3[] vertices;

    private void Awake()
    {
        waterMesh = this.GetComponent<MeshFilter>().mesh;
        vertices = waterMesh.vertices;
    }

    private void Update()
    {
        if (vertices == null) return;

        Vector3[] newVertices = new Vector3[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            float newY = Waves.instance.GetWaveHeight(vertices[i].x, vertices[i].z); // Recalculate vertex height at each position
            newVertices[i] = new Vector3(vertices[i].x, newY, vertices[i].z);
        }

        waterMesh.vertices = newVertices;
        waterMesh.RecalculateNormals();
        waterMesh.RecalculateBounds();
    }
}
