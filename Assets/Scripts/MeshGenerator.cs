using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using mattatz.MeshSmoothingSystem;

[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(VoxelData)), RequireComponent(typeof(MeshCollider))]
public class MeshGenerator : MonoBehaviour
{
    private List<Vector3> _vertices = new List<Vector3>();
    private List<Vector3> _unuseVertices = new List<Vector3>();
    private List<int> _triangles = new List<int>();
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    [SerializeField]
    private bool noise = false;
    [SerializeField]
    private float noiseTreshold = 1.5f;
    [SerializeField]
    private bool smooth = false;
    [SerializeField]
    private bool merge = false;
    [SerializeField]
    private bool subdivide = false;

    public void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        _meshFilter = GetComponent<MeshFilter>();

        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;

        CreateShape();
        UpdateMesh();
    }
    [ContextMenu("Update")]
    public void Update()
    {
        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        _vertices = new List<Vector3>();
        _unuseVertices = new List<Vector3>();
        _triangles = new List<int>();
        #region cube :)
        /*vertices = new Vector3[]
        {
            new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(1, 0, 1),
            new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 0), new Vector3(1, 1, 1)
        };

        triangles = new int[]
        {
             0, 2, 1
            ,1, 2, 3

            ,4, 5, 6
            ,5, 7, 6

            ,0, 4, 2
            ,4, 6, 2

            ,2, 6, 3
            ,6, 7, 3

            ,3, 7, 1
            ,7, 5, 1

            ,1, 5, 0
            ,5, 4, 0
        };*/
        #endregion
        #region grid :)
        /*uint w = 10;
        uint d = 10;

        vertices = new Vector3[] {};
        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < d; z++)
            {
                vertices = vertices.Concat(new Vector3[]
                {
                    new Vector3(0, 0, 0) + new Vector3(x, 0, z),
                    new Vector3(0, 0, 1) + new Vector3(x, 0, z),
                    new Vector3(1, 0, 0) + new Vector3(x, 0, z),
                    new Vector3(1, 0, 1) + new Vector3(x, 0, z)
                }).ToArray();
            }
        }

        triangles = new int[] {};
        for (int i = 0; i < w * d; i++)
        {
            triangles = triangles.Concat(new int[] { 0 + i * 4, 1 + i * 4, 2 + i * 4, 1 + i * 4, 3 + i * 4, 2 + i * 4 }).ToArray();
        }*/
        #endregion
        #region voxels ;)
        VoxelData voxelData = GetComponent<VoxelData>();

        for (int x = 0; x < voxelData.DataSize.x; x++)
        {
            for (int y = 0; y < voxelData.DataSize.y; y++)
            {
                for (int z = 0; z < voxelData.DataSize.z; z++)
                {
                    if (voxelData.Get(x, y, z))
                    {
                        if (!voxelData.Get(x + 1, y, z)) AddCubeFace(new Vector3(x, y, z), Direction.Right, voxelData);
                        if (!voxelData.Get(x - 1, y, z)) AddCubeFace(new Vector3(x, y, z), Direction.Left, voxelData);
                        if (!voxelData.Get(x, y + 1, z)) AddCubeFace(new Vector3(x, y, z), Direction.Up, voxelData);
                        if (!voxelData.Get(x, y - 1, z)) AddCubeFace(new Vector3(x, y, z), Direction.Down, voxelData);
                        if (!voxelData.Get(x, y, z + 1)) AddCubeFace(new Vector3(x, y, z), Direction.Forward, voxelData);
                        if (!voxelData.Get(x, y, z - 1)) AddCubeFace(new Vector3(x, y, z), Direction.Backward, voxelData);
                    }
                }
            }
        }
        #endregion
    }

    private enum Direction
    {
        Up, Down, Forward, Backward, Right, Left
    }

    private void AddCubeFace(Vector3 position, Direction direction, VoxelData data)
    {
        if (subdivide)
        {
            switch (direction)
            {
                case Direction.Up:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+0, _vertices.Count+1, _vertices.Count+2,
                    _vertices.Count+1, _vertices.Count+3, _vertices.Count+2,

                    _vertices.Count+4, _vertices.Count+5, _vertices.Count+6,
                    _vertices.Count+5, _vertices.Count+7, _vertices.Count+6,

                    _vertices.Count+8, _vertices.Count+9, _vertices.Count+10,
                    _vertices.Count+9, _vertices.Count+11, _vertices.Count+10,

                    _vertices.Count+12, _vertices.Count+13, _vertices.Count+14,
                    _vertices.Count+13, _vertices.Count+15, _vertices.Count+14,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x + 0, position.y + 1, position.z + 0),
                    new Vector3(position.x + 0, position.y + 1, position.z + 0.5f),
                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 0),
                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 0.5f),

                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 0.5f),
                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 1),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0.5f),
                    new Vector3(position.x + 1, position.y + 1, position.z + 1),

                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 0),
                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 0.5f),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0.5f),

                    new Vector3(position.x + 0, position.y + 1, position.z + 0.5f),
                    new Vector3(position.x + 0, position.y + 1, position.z + 1),
                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 0.5f),
                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 1),
                }).ToList();
                    break;
                case Direction.Down:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+0, _vertices.Count+2, _vertices.Count+1,
                    _vertices.Count+1, _vertices.Count+2, _vertices.Count+3,

                    _vertices.Count+4, _vertices.Count+6, _vertices.Count+5,
                    _vertices.Count+5, _vertices.Count+6, _vertices.Count+7,

                    _vertices.Count+8, _vertices.Count+10, _vertices.Count+9,
                    _vertices.Count+9, _vertices.Count+10, _vertices.Count+11,

                    _vertices.Count+12, _vertices.Count+14, _vertices.Count+13,
                    _vertices.Count+13, _vertices.Count+14, _vertices.Count+15,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x+0, position.y +0, position.z +0),
                    new Vector3(position.x+0, position.y +0, position.z +0.5f),
                    new Vector3(position.x+0.5f, position.y +0, position.z +0),
                    new Vector3(position.x+0.5f, position.y +0, position.z +0.5f),

                    new Vector3(position.x+0.5f, position.y +0, position.z +0.5f),
                    new Vector3(position.x+0.5f, position.y +0, position.z +1),
                    new Vector3(position.x+1, position.y +0, position.z +0.5f),
                    new Vector3(position.x+1, position.y +0, position.z +1),

                    new Vector3(position.x+0.5f, position.y +0, position.z +0),
                    new Vector3(position.x+0.5f, position.y +0, position.z +0.5f),
                    new Vector3(position.x+1, position.y +0, position.z +0),
                    new Vector3(position.x+1, position.y +0, position.z +0.5f),

                    new Vector3(position.x+0, position.y +0, position.z +0.5f),
                    new Vector3(position.x+0, position.y +0, position.z +1),
                    new Vector3(position.x+0.5f, position.y +0, position.z +0.5f),
                    new Vector3(position.x+0.5f, position.y +0, position.z +1),
                }).ToList();
                    break;
                case Direction.Forward:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+1, _vertices.Count+3, _vertices.Count+0,
                    _vertices.Count+3, _vertices.Count+2, _vertices.Count+0,

                    _vertices.Count+5, _vertices.Count+7, _vertices.Count+4,
                    _vertices.Count+7, _vertices.Count+6, _vertices.Count+4,

                    _vertices.Count+9, _vertices.Count+11, _vertices.Count+8,
                    _vertices.Count+11, _vertices.Count+10, _vertices.Count+8,

                    _vertices.Count+13, _vertices.Count+15, _vertices.Count+12,
                    _vertices.Count+15, _vertices.Count+14, _vertices.Count+12,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x + 0, position.y + 0, position.z + 1),
                    new Vector3(position.x + 0.5f, position.y + 0, position.z + 1),
                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 1),
                    new Vector3(position.x + 0.5f, position.y + 0.5f, position.z + 1),

                    new Vector3(position.x + 0.5f, position.y + 0.5f, position.z + 1),
                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 1),
                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 1),
                    new Vector3(position.x + 1, position.y + 1, position.z + 1),

                    new Vector3(position.x + 0.5f, position.y + 0, position.z + 1),
                    new Vector3(position.x + 1, position.y + 0, position.z + 1),
                    new Vector3(position.x + 0.5f, position.y + 0.5f, position.z + 1),
                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 1),

                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 1),
                    new Vector3(position.x + 0.5f, position.y + 0.5f, position.z + 1),
                    new Vector3(position.x + 0, position.y + 1, position.z + 1),
                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 1),
                }).ToList();
                    break;
                case Direction.Backward:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+0, _vertices.Count+2, _vertices.Count+1,
                    _vertices.Count+2, _vertices.Count+3, _vertices.Count+1,

                    _vertices.Count+4, _vertices.Count+6, _vertices.Count+5,
                    _vertices.Count+6, _vertices.Count+7, _vertices.Count+5,

                    _vertices.Count+8, _vertices.Count+10, _vertices.Count+9,
                    _vertices.Count+10, _vertices.Count+11, _vertices.Count+9,

                    _vertices.Count+12, _vertices.Count+14, _vertices.Count+13,
                    _vertices.Count+14, _vertices.Count+15, _vertices.Count+13,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x + 0, position.y + 0, position.z + 0),
                    new Vector3(position.x + 0.5f, position.y + 0, position.z + 0),
                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 0),
                    new Vector3(position.x + 0.5f, position.y + 0.5f, position.z + 0),

                    new Vector3(position.x + 0.5f, position.y + 0.5f, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 0),
                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 0),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0),

                    new Vector3(position.x + 0.5f, position.y + 0, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0, position.z + 0),
                    new Vector3(position.x + 0.5f, position.y + 0.5f, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 0),

                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 0),
                    new Vector3(position.x + 0.5f, position.y + 0.5f, position.z + 0),
                    new Vector3(position.x + 0, position.y + 1, position.z + 0),
                    new Vector3(position.x + 0.5f, position.y + 1, position.z + 0),

                }).ToList();
                    break;
                case Direction.Right:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+0, _vertices.Count+2, _vertices.Count+1,
                    _vertices.Count+2, _vertices.Count+3, _vertices.Count+1,

                    _vertices.Count+4, _vertices.Count+6, _vertices.Count+5,
                    _vertices.Count+6, _vertices.Count+7, _vertices.Count+5,

                    _vertices.Count+8, _vertices.Count+10, _vertices.Count+9,
                    _vertices.Count+10, _vertices.Count+11, _vertices.Count+9,

                    _vertices.Count+12, _vertices.Count+14, _vertices.Count+13,
                    _vertices.Count+14, _vertices.Count+15, _vertices.Count+13,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x + 1, position.y + 0, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0, position.z + 0.5f),
                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 0.5f),

                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 0.5f),
                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 1),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0.5f),
                    new Vector3(position.x + 1, position.y + 1, position.z + 1),

                    new Vector3(position.x + 1, position.y + 0, position.z + 0.5f),
                    new Vector3(position.x + 1, position.y + 0, position.z + 1),
                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 0.5f),
                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 1),

                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0.5f, position.z + 0.5f),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0.5f),
                }).ToList();
                    break;
                case Direction.Left:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+1, _vertices.Count+3, _vertices.Count+0,
                    _vertices.Count+3, _vertices.Count+2, _vertices.Count+0,

                    _vertices.Count+5, _vertices.Count+7, _vertices.Count+4,
                    _vertices.Count+7, _vertices.Count+6, _vertices.Count+4,

                    _vertices.Count+9, _vertices.Count+11, _vertices.Count+8,
                    _vertices.Count+11, _vertices.Count+10, _vertices.Count+8,

                    _vertices.Count+13, _vertices.Count+15, _vertices.Count+12,
                    _vertices.Count+15, _vertices.Count+14, _vertices.Count+12,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x + 0, position.y + 0, position.z + 0),
                    new Vector3(position.x + 0, position.y + 0, position.z + 0.5f),
                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 0),
                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 0.5f),

                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 0.5f),
                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 1),
                    new Vector3(position.x + 0, position.y + 1, position.z + 0.5f),
                    new Vector3(position.x + 0, position.y + 1, position.z + 1),

                    new Vector3(position.x + 0, position.y + 0, position.z + 0.5f),
                    new Vector3(position.x + 0, position.y + 0, position.z + 1),
                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 0.5f),
                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 1),

                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 0),
                    new Vector3(position.x + 0, position.y + 0.5f, position.z + 0.5f),
                    new Vector3(position.x + 0, position.y + 1, position.z + 0),
                    new Vector3(position.x + 0, position.y + 1, position.z + 0.5f),
                }).ToList();
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (!data.CheckPositionCorrectness(position + new Vector3(-1, 0, 0))) _unuseVertices = _unuseVertices.Concat(new List<Vector3>()
            {
                    new Vector3(position.x + 0, position.y + 0, position.z + 0),
                    new Vector3(position.x + 0, position.y + 0, position.z + 1),
                    new Vector3(position.x + 0, position.y + 1, position.z + 0),
                    new Vector3(position.x + 0, position.y + 1, position.z + 1),
            }).ToList();
            if (!data.CheckPositionCorrectness(position + new Vector3(1, 0, 0))) _unuseVertices = _unuseVertices.Concat(new List<Vector3>()
            {
                    new Vector3(position.x + 1, position.y + 0, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0, position.z + 1),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0),
                    new Vector3(position.x + 1, position.y + 1, position.z + 1),
            }).ToList();
            if (!data.CheckPositionCorrectness(position + new Vector3(0, 1, 0))) _unuseVertices = _unuseVertices.Concat(new List<Vector3>()
            {
                    new Vector3(position.x + 0, position.y + 1, position.z + 0),
                    new Vector3(position.x + 0, position.y + 1, position.z + 1),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0),
                    new Vector3(position.x + 1, position.y + 1, position.z + 1),
            }).ToList();
            if (!data.CheckPositionCorrectness(position + new Vector3(0, -1, 0))) _unuseVertices = _unuseVertices.Concat(new List<Vector3>()
            {
                    new Vector3(position.x + 0, position.y + 0, position.z + 0),
                    new Vector3(position.x + 0, position.y + 0, position.z + 1),
                    new Vector3(position.x + 1, position.y + 0, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0, position.z + 1),
            }).ToList();
            if (!data.CheckPositionCorrectness(position + new Vector3(0, 0, 1))) _unuseVertices = _unuseVertices.Concat(new List<Vector3>()
            {
                    new Vector3(position.x + 0, position.y + 0, position.z + 1),
                    new Vector3(position.x + 1, position.y + 0, position.z + 1),
                    new Vector3(position.x + 0, position.y + 1, position.z + 1),
                    new Vector3(position.x + 1, position.y + 1, position.z + 1),
            }).ToList();
            if (!data.CheckPositionCorrectness(position + new Vector3(0, 0, -1))) _unuseVertices = _unuseVertices.Concat(new List<Vector3>()
            {
                    new Vector3(position.x + 0, position.y + 0, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0, position.z + 0),
                    new Vector3(position.x + 0, position.y + 1, position.z + 0),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0),
            }).ToList();

            switch (direction)
            {
                case Direction.Up:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+0, _vertices.Count+1, _vertices.Count+2,
                    _vertices.Count+1, _vertices.Count+3, _vertices.Count+2,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x + 0, position.y + 1, position.z + 0),
                    new Vector3(position.x + 0, position.y + 1, position.z + 1),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0),
                    new Vector3(position.x + 1, position.y + 1, position.z + 1),
                }).ToList();
                    break;
                case Direction.Down:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+0, _vertices.Count+2, _vertices.Count+1,
                    _vertices.Count+1, _vertices.Count+2, _vertices.Count+3,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x + 0, position.y + 0, position.z + 0),
                    new Vector3(position.x + 0, position.y + 0, position.z + 1),
                    new Vector3(position.x + 1, position.y + 0, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0, position.z + 1),
                }).ToList();
                    break;
                case Direction.Forward:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+1, _vertices.Count+3, _vertices.Count+0,
                    _vertices.Count+3, _vertices.Count+2, _vertices.Count+0,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x + 0, position.y + 0, position.z + 1),
                    new Vector3(position.x + 1, position.y + 0, position.z + 1),
                    new Vector3(position.x + 0, position.y + 1, position.z + 1),
                    new Vector3(position.x + 1, position.y + 1, position.z + 1),
                }).ToList();
                    break;
                case Direction.Backward:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+0, _vertices.Count+2, _vertices.Count+1,
                    _vertices.Count+2, _vertices.Count+3, _vertices.Count+1,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x + 0, position.y + 0, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0, position.z + 0),
                    new Vector3(position.x + 0, position.y + 1, position.z + 0),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0),

                }).ToList();
                    break;
                case Direction.Right:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+0, _vertices.Count+2, _vertices.Count+1,
                    _vertices.Count+2, _vertices.Count+3, _vertices.Count+1,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x + 1, position.y + 0, position.z + 0),
                    new Vector3(position.x + 1, position.y + 0, position.z + 1),
                    new Vector3(position.x + 1, position.y + 1, position.z + 0),
                    new Vector3(position.x + 1, position.y + 1, position.z + 1),
                }).ToList();
                    break;
                case Direction.Left:
                    _triangles = _triangles.Concat(new List<int>()
                {
                    _vertices.Count+1, _vertices.Count+3, _vertices.Count+0,
                    _vertices.Count+3, _vertices.Count+2, _vertices.Count+0,
                }).ToList();

                    _vertices = _vertices.Concat(new List<Vector3>()
                {
                    new Vector3(position.x + 0, position.y + 0, position.z + 0),
                    new Vector3(position.x + 0, position.y + 0, position.z + 1),
                    new Vector3(position.x + 0, position.y + 1, position.z + 0),
                    new Vector3(position.x + 0, position.y + 1, position.z + 1),
                }).ToList();
                    break;
                default:
                    break;
            }
        }
    }
    private void AddCube(Vector3 position)
    {
        _triangles = _triangles.Concat(new List<int>()
        {
             _vertices.Count+0, _vertices.Count+2, _vertices.Count+1
            ,_vertices.Count+1, _vertices.Count+2, _vertices.Count+3
            ,_vertices.Count+4, _vertices.Count+5, _vertices.Count+6
            ,_vertices.Count+5, _vertices.Count+7, _vertices.Count+6
            ,_vertices.Count+0, _vertices.Count+4, _vertices.Count+2
            ,_vertices.Count+4, _vertices.Count+6, _vertices.Count+2
            ,_vertices.Count+2, _vertices.Count+6, _vertices.Count+3
            ,_vertices.Count+6, _vertices.Count+7, _vertices.Count+3
            ,_vertices.Count+3, _vertices.Count+7, _vertices.Count+1
            ,_vertices.Count+7, _vertices.Count+5, _vertices.Count+1
            ,_vertices.Count+1, _vertices.Count+5, _vertices.Count+0
            ,_vertices.Count+5, _vertices.Count+4, _vertices.Count+0
        }).ToList();

        _vertices = _vertices.Concat(new List<Vector3>()
        {
            new Vector3(position.x+0, position.y +0, position.z +0), new Vector3(position.x+0, position.y +0, position.z +1), new Vector3(position.x+1, position.y +0, position.z +0), new Vector3(position.x+1, position.y +0, position.z +1),
            new Vector3(position.x+0, position.y +1,position.z + 0), new Vector3(position.x+0, position.y +1, position.z +1), new Vector3(position.x+1, position.y +1, position.z +0), new Vector3(position.x+1, position.y + 1, position.z +1)
        }).ToList();
    }

    private void UpdateMesh()
    {
        _mesh = new Mesh();
        _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshFilter>().sharedMesh = _mesh;

        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.RecalculateNormals();

        if (merge) _mesh.WeldVertices();
        if (smooth) _mesh.LaplacianFilter(1);
        if (noise) _mesh.Randomise(noiseTreshold);
        //if (smooth) mesh.LaplacianFilter(1);

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = _mesh;
    }
}
public static class MeshWelder
{
    public static Mesh Randomise(this Mesh mesh, float value)
    {
        var verts = mesh.vertices;
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            verts[i] = mesh.vertices[i] + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * value;
        }
        mesh.vertices = verts;
        return mesh;
    }
    public static Mesh WeldVertices(this Mesh aMesh, float aMaxDelta = 0.01f, float roundFctr = 1000f)
    {
        var verts = aMesh.vertices;
        var normals = aMesh.normals;
        var uvs = aMesh.uv;
        Dictionary<Vector3, int> duplicateHashTable = new Dictionary<Vector3, int>();
        List<int> newVerts = new List<int>();
        int[] map = new int[verts.Length];

        //create mapping and find duplicates, dictionaries are like hashtables, mean fast
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] = new Vector3(
             Mathf.Round(roundFctr * verts[i].x) / roundFctr,
             Mathf.Round(roundFctr * verts[i].y) / roundFctr,
             Mathf.Round(roundFctr * verts[i].z) / roundFctr);
            if (!duplicateHashTable.ContainsKey(verts[i]))
            {
                duplicateHashTable.Add(verts[i], newVerts.Count);
                map[i] = newVerts.Count;
                newVerts.Add(i);
            }
            else
            {
                map[i] = duplicateHashTable[verts[i]];
            }
        }

        // create new vertices
        var verts2 = new Vector3[newVerts.Count];
        var normals2 = new Vector3[newVerts.Count];
        var uvs2 = new Vector2[newVerts.Count];
        for (int i = 0; i < newVerts.Count; i++)
        {
            int a = newVerts[i];
            verts2[i] = verts[a];
            normals2[i] = normals[a];
            //uvs2[i] = uvs[a];
        }
        // map the triangle to the new vertices
        var tris = aMesh.triangles;
        for (int i = 0; i < tris.Length; i++)
        {
            tris[i] = map[tris[i]];
        }
        aMesh.triangles = tris;
        aMesh.vertices = verts2;
        aMesh.normals = normals2;
        aMesh.uv = uvs2;

        aMesh.RecalculateBounds();
        aMesh.RecalculateNormals();

        return aMesh;
    }

}