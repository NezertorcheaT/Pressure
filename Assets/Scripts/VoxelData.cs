using UnityEngine;
using UnityEngine.Serialization;

public class VoxelData : MonoBehaviour
{
    private bool[,,] _data = new bool[,,]
    {
        {
            {
                true, true, true
            },
            {
                false, true, false
            },
            {
                false, false, false
            }
        },
        {
            {
                false, false, false
            },
            {
                false, false, true
            },
            {
                false, false, true
            }
        },
        {
            {
                false, false, true
            },
            {
                false, false, true
            },
            {
                true, false, false
            }
        },
        {
            {
                false, false, true
            },
            {
                false, true, true
            },
            {
                true, false, false
            }
        }
    };

    [FormerlySerializedAs("wegth")] [SerializeField]
    private uint weight = 10;

    [SerializeField] private uint height = 10;
    [SerializeField] private uint depth = 10;
    [SerializeField, Range(0f, 1f)] private float treshold = 0.5f;
    [SerializeField, Min(0.001f)] private float scale = 0.5f;
    [SerializeField] private bool unknownPos = true;

    [ContextMenu("Update")]
    public void UpdateBox()
    {
        _data = new bool[weight, height, depth];

        for (var x = 0; x < DataSize.x; x++)
        {
            for (var y = 0; y < DataSize.y; y++)
            {
                for (var z = 0; z < DataSize.z; z++)
                {
                    Set(PerlinNoise3D(new Vector3((float) (x + transform.position.x) / (float) weight,
                        (float) (y + transform.position.y) / (float) height,
                        (float) (z + transform.position.z) / (float) depth) * scale) > treshold, x, y, z);
                }
            }
        }
    }

    public Vector3Int DataSize => new Vector3Int(_data.GetLength(0), _data.GetLength(1), _data.GetLength(2));

    public bool Get(Vector3Int position)
    {
        return !CheckPositionCorrectness(position.x, position.y, position.z)
            ? unknownPos
            : _data[position.x, position.y, position.z];
    }

    public bool Get(Vector3 position) => Get(new Vector3Int((int) position.x, (int) position.y, (int) position.z));
    public bool Get(int x, int y, int z) => Get(new Vector3Int(x, y, z));

    public bool CheckPositionCorrectness(int x, int y, int z) =>
        x >= 0 && y >= 0 && z >= 0 && x < DataSize.x && y < DataSize.y && z < DataSize.z;

    public bool CheckPositionCorrectness(Vector3Int position) =>
        CheckPositionCorrectness(position.x, position.y, position.z);

    public bool CheckPositionCorrectness(Vector3 position) =>
        CheckPositionCorrectness((int) position.x, (int) position.y, (int) position.z);

    public void Set(bool b, int x, int y, int z)
    {
        if (CheckPositionCorrectness(x, y, z))
            _data[x, y, z] = b;
    }

    public void Set(bool b, Vector3Int position) => Set(b, position.x, position.y, position.z);
    public void Set(bool b, Vector3 position) => Set(b, (int) position.x, (int) position.y, (int) position.z);

    public static float PerlinNoise3D(float x, float y, float z) => (Mathf.PerlinNoise(x, y) + Mathf.PerlinNoise(y, z) +
                                                                     Mathf.PerlinNoise(x, z) + Mathf.PerlinNoise(y, x) +
                                                                     Mathf.PerlinNoise(z, y) +
                                                                     Mathf.PerlinNoise(z, x)) / 6f;

    public static float PerlinNoise3D(Vector3 position) => PerlinNoise3D(position.x, position.y, position.z);
}