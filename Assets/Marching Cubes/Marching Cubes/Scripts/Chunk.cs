using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using Unity.VisualScripting;

public class Chunk
{

    public Vector3 centre;
    public float size;
    public Mesh mesh;

    public ComputeBuffer pointsBuffer;
    int numPointsPerAxis;
    public MeshFilter filter;
    MeshRenderer renderer;
    MeshCollider collider;
    public bool terra;
    public Vector3Int id;

    // Mesh processing
    Dictionary<int2, int> vertexIndexMap;
    List<Vector3> processedVertices;
    List<Vector3> processedNormals;
    List<int> processedTriangles;


    public Chunk(Vector3Int coord, Vector3 centre, float size, int numPointsPerAxis, GameObject meshHolder)
    {
        this.id = coord;
        this.centre = centre;
        this.size = size;
        this.numPointsPerAxis = numPointsPerAxis;

        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        int numPointsTotal = numPointsPerAxis * numPointsPerAxis * numPointsPerAxis;
        ComputeHelper.CreateStructuredBuffer<PointData>(ref pointsBuffer, numPointsTotal);

        // Mesh rendering and collision components
        filter = meshHolder.AddComponent<MeshFilter>();
        renderer = meshHolder.AddComponent<MeshRenderer>();


        filter.mesh = mesh;
        collider = renderer.gameObject.AddComponent<MeshCollider>();

        vertexIndexMap = new Dictionary<int2, int>();
        processedVertices = new List<Vector3>();
        processedNormals = new List<Vector3>();
        processedTriangles = new List<int>();
    }

    public void CreateMesh(VertexData[] vertexData, int numVertices, bool useFlatShading)
    {

        vertexIndexMap.Clear();
        processedVertices.Clear();
        processedNormals.Clear();
        processedTriangles.Clear();

        int triangleIndex = 0;

        for (int i = 0; i < numVertices; i++)
        {
            VertexData data = vertexData[i];

            int sharedVertexIndex;
            if (!useFlatShading && vertexIndexMap.TryGetValue(data.id, out sharedVertexIndex))
            {
                processedTriangles.Add(sharedVertexIndex);
            }
            else
            {
                if (!useFlatShading)
                {
                    vertexIndexMap.Add(data.id, triangleIndex);
                }
                processedVertices.Add(data.position);
                processedNormals.Add(data.normal);
                processedTriangles.Add(triangleIndex);
                triangleIndex++;
            }
        }

        collider.sharedMesh = null;

        mesh.Clear();
        mesh.SetVertices(processedVertices);
        mesh.SetTriangles(processedTriangles, 0, true);

        if (useFlatShading)
        {
            mesh.RecalculateNormals();
        }
        else
        {
            mesh.SetNormals(processedNormals);
        }

        Vector3[] vertices = mesh.vertices;
        int[] tris = mesh.triangles;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < tris.Length; i += 3)
        {

            Vector3 a = vertices[tris[i]];
            Vector3 b = vertices[tris[i + 1]];
            Vector3 c = vertices[tris[i + 2]];
            Vector3 side1 = b - a;
            Vector3 side2 = c - a;
            Vector3 N = Vector3.Cross(side1, side2);

            N = new Vector3(Mathf.Abs(N.normalized.x), Mathf.Abs(N.normalized.y), Mathf.Abs(N.normalized.z));



            if (N.x > N.y && N.x > N.z)
            {
                uvs[tris[i]] = new Vector2(vertices[tris[i]].z, vertices[tris[i]].y)/50;
                uvs[tris[i + 1]] = new Vector2(vertices[tris[i + 1]].z, vertices[tris[i + 1]].y) / 50;
                uvs[tris[i + 2]] = new Vector2(vertices[tris[i + 2]].z, vertices[tris[i + 2]].y) / 50;
            }
            else if (N.y > N.x && N.y > N.z)
            {
                uvs[tris[i]] = new Vector2(vertices[tris[i]].x, vertices[tris[i]].z) / 50;
                uvs[tris[i + 1]] = new Vector2(vertices[tris[i + 1]].x, vertices[tris[i + 1]].z) / 50;
                uvs[tris[i + 2]] = new Vector2(vertices[tris[i + 2]].x, vertices[tris[i + 2]].z) / 50;
            }
            else if (N.z > N.x && N.z > N.y)
            {
                uvs[tris[i]] = new Vector2(vertices[tris[i]].x, vertices[tris[i]].y) / 50;
                uvs[tris[i + 1]] = new Vector2(vertices[tris[i + 1]].x, vertices[tris[i + 1]].y) / 50;
                uvs[tris[i + 2]] = new Vector2(vertices[tris[i + 2]].x, vertices[tris[i + 2]].y) / 50;
            }

        }

        mesh.uv = uvs;
        collider.sharedMesh = mesh;
    }

    public struct PointData
    {
        public Vector3 position;
        public Vector3 normal;
        public float density;
    }

    public void AddCollider()
    {
        collider.sharedMesh = mesh;
    }

    public void SetMaterial(Material material)
    {
        renderer.material = material;
    }

    public void Release()
    {
        ComputeHelper.Release(pointsBuffer);
    }

    public void DrawBoundsGizmo(Color col)
    {
        Gizmos.color = col;
        Gizmos.DrawWireCube(centre, Vector3.one * size);
    }
}