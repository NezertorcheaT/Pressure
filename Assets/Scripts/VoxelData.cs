using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VoxelData : MonoBehaviour
{
    private bool[,,] data = new bool[,,] {
        {
            {
                true,true,true
            },
            {
                false,true,false
            },
            {
                false,false,false
            }
        },
        {
            {
                false,false,false
            },
            {
                false,false,true
            },
            {
                false,false,true
            }
        },
        {
            {
                false,false,true
            },
            {
                false,false,true
            },
            {
                true,false,false
            }
        },
        {
            {
                false,false,true
            },
            {
                false,true,true
            },
            {
                true,false,false
            }
        }
    };

    [SerializeField]
    private uint wegth = 10;
    [SerializeField]
    private uint height = 10;
    [SerializeField]
    private uint depth = 10;
    [SerializeField, Range(0f, 1f)]
    private float treshold = 0.5f;
    [SerializeField, Min(0.001f)]
    private float scale = 0.5f;
    [SerializeField]
    private bool unknownPos = true;

    [ContextMenu("Update")]
    public void UpdateBox()
    {
        data = new bool[wegth, height, depth];

        for (int x = 0; x < DataSize.x; x++)
        {
            for (int y = 0; y < DataSize.y; y++)
            {
                for (int z = 0; z < DataSize.z; z++)
                {
                    if (PerlinNoise3D(new Vector3((float)(x + transform.position.x) / (float)wegth, (float)(y + transform.position.y) / (float)height, (float)(z + transform.position.z) / (float)depth) * scale) > treshold)
                    {
                        Set(true, x, y, z);
                    }
                    else
                    {
                        Set(false, x, y, z);
                    }
                }
            }
        }
    }

    public Vector3Int DataSize => new Vector3Int(data.GetLength(0), data.GetLength(1), data.GetLength(2));

    public bool Get(Vector3Int position)
    {
        if (!CheckPositionCorrectness(position.x, position.y, position.z))
        {
            return unknownPos;
        }
        return data[position.x, position.y, position.z];
    }
    public bool Get(Vector3 position) => Get(new Vector3Int((int)position.x, (int)position.y, (int)position.z));
    public bool Get(int x, int y, int z) => Get(new Vector3Int(x, y, z));

    public bool CheckPositionCorrectness(int x, int y, int z)
    {
        if (x < 0 || y < 0 || z < 0 || x >= DataSize.x || y >= DataSize.y || z >= DataSize.z)
        {
            return false;
        }
        return true;
    }
    public bool CheckPositionCorrectness(Vector3Int position) => CheckPositionCorrectness(position.x, position.y, position.z);
    public bool CheckPositionCorrectness(Vector3 position) => CheckPositionCorrectness((int)position.x, (int)position.y, (int)position.z);

    public void Set(bool b, int x, int y, int z)
    {
        if (CheckPositionCorrectness(x, y, z))
            data[x, y, z] = b;
    }
    public void Set(bool b, Vector3Int position) => Set(b, position.x, position.y, position.z);
    public void Set(bool b, Vector3 position) => Set(b, (int)position.x, (int)position.y, (int)position.z);

    public static float PerlinNoise3D(float x, float y, float z) => (Mathf.PerlinNoise(x, y) + Mathf.PerlinNoise(y, z) + Mathf.PerlinNoise(x, z) + Mathf.PerlinNoise(y, x) + Mathf.PerlinNoise(z, y) + Mathf.PerlinNoise(z, x)) / 6f;
    public static float PerlinNoise3D(Vector3 position) => PerlinNoise3D(position.x, position.y, position.z);
}
