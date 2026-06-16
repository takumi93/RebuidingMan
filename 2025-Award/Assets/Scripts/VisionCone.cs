using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class VisionCone : MonoBehaviour
{
    [SerializeField] private float distance = 10f;
    [SerializeField] private float angle = 60f;
    [SerializeField] private int segments = 20;

    private MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        GenerateMesh();
    }

    /// <summary>
    /// ‹“_”ÍˆÍ‚Ìİ’è
    /// </summary>
    /// <param name="newDistance"></param>
    /// <param name="newAngle"></param>
    public void Setup(float newDistance, float newAngle)
    {
        distance = newDistance;
        angle = newAngle;

        GenerateMesh();
    }

    /// <summary>
    /// ƒƒbƒVƒ…‚Ì¶¬
    /// </summary>
    private void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[segments + 2];

        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -angle * 0.5f + angle * i / segments;

            float rad = currentAngle * Mathf.Deg2Rad;

            vertices[i + 1] =
                new Vector3(
                    Mathf.Sin(rad) * distance,
                    0f,
                    Mathf.Cos(rad) * distance);
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}