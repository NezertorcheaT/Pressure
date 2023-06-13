using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunks : MonoBehaviour
{
    [SerializeField]
    private GameObject meshGeneratorPrefab;
    [SerializeField]
    private Vector3Int size;
    private List<GameObject> chuncs;

    [ContextMenu("Update")]
    private void Start()
    {
        if (chuncs != null && chuncs.Count != 0)
        {
            foreach (var chunc in chuncs)
            {
                Destroy(chunc);
            }
        }

        chuncs = new List<GameObject>();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    GameObject meshGenerator = Instantiate(meshGeneratorPrefab);
                    meshGenerator.layer = gameObject.layer;
                    meshGenerator.transform.SetParent(null);

                    MeshGenerator generator = meshGenerator.GetComponent<MeshGenerator>();
                    VoxelData data = meshGenerator.GetComponent<VoxelData>();
                    data.UpdateBox();

                    meshGenerator.transform.position = new Vector3(x * data.DataSize.x, y * data.DataSize.y, z * data.DataSize.z);

                    data.UpdateBox();
                    generator.update();
                    chuncs.Add(meshGenerator);
                }
            }
        }
    }
}
