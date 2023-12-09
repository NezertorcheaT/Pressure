using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Generation.Placers
{
    [CreateAssetMenu(fileName = "RayCast Placer", menuName = "Generation Placers/Ray Casting", order = 1)]
    public class RayCastGenerationPlacer : ScriptableObject, IGenerationPlacer
    {
        async Task IGenerationPlacer.PlaceRandom(GenerationBuildable buildable, Action<GenerationBounds> ImmerseTerrain,
            Func<UnityEngine.Object, Vector3, Quaternion, Transform, GameObject> InstantiatePrefab, float boundsSize)
        {
            await Task.Delay(1);

            RaycastHit rh;
            while (true)
            {
                var hit = Physics.RaycastAll(
                    new Vector3(
                        Random.Range(-boundsSize, boundsSize),
                        Random.Range(-boundsSize, boundsSize),
                        Random.Range(-boundsSize, boundsSize)
                    ),
                    new Vector3(
                        Random.Range(-1.0f, 1.0f),
                        Random.Range(-1.0f, 1.0f),
                        Random.Range(-1.0f, 1.0f)
                    ).normalized,
                    boundsSize
                );

                var ind = Random.Range(1, hit.Length - 2);
                if (hit.Length <= 3 || hit[ind].distance <= 0.1f ||
                    hit[ind].collider.gameObject.CompareTag("Submarine")) continue;
                if ((hit[ind].point + hit[ind].normal * buildable.normalOffset).magnitude >= boundsSize / 2f) continue;

                rh = hit[ind];
                break;
            }

            var gb = InstantiatePrefab(buildable.prefab, rh.point + rh.normal * buildable.normalOffset,
                buildable.normalRotate ? Quaternion.LookRotation(rh.normal) : Quaternion.identity,
                null).GetComponent<GenerationBounds>();

            if (buildable.prefab.Immerse) ImmerseTerrain?.Invoke(gb);
        }
    }
}