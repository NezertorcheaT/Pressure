using UnityEngine;
using Zenject;

namespace Installers
{
    public class TerrainGenerationInstaller : MonoInstaller
    {
        [Header("Init Settings")] [SerializeField]
        private int numChunks = 10;

        [Space] [SerializeField] private int numPointsPerAxis = 35;
        [SerializeField] private float boundsSize = 500;
        [SerializeField] private float isoLevel = 0f;
        [SerializeField] private bool useFlatShading = false;
        [Space] [SerializeField] private float noiseScale = 1.6f;
        [SerializeField] private float noiseHeightMultiplier = 0.9f;
        [SerializeField] private bool blurMap = true;
        [SerializeField] private int blurRadius = 7;
        [Space] [SerializeField] private int layer = 6;

        [Header("References")] [SerializeField]
        private ComputeShader meshCompute;

        [SerializeField] private ComputeShader densityCompute;
        [SerializeField] private ComputeShader blurCompute;
        [SerializeField] private ComputeShader editCompute;
        [Space] [SerializeField] private Material material;

        [Space] [SerializeField, Min(0)] private int torpedoesCount;
        [SerializeField] private GameObject torpedoPrefab;
        [SerializeField, Min(0)] private int stalagtiteCount;
        [SerializeField] private GameObject stalagtitePrefab;

        private GenTest _terrain;

        public override void InstallBindings()
        {
            _terrain = Container.InstantiateComponentOnNewGameObject<GenTest>("Terrain");

            _terrain.gameObject.layer = layer;
            _terrain.numChunks = numChunks;
            _terrain.numPointsPerAxis = numPointsPerAxis;
            _terrain.boundsSize = boundsSize;
            _terrain.isoLevel = isoLevel;
            _terrain.useFlatShading = useFlatShading;
            _terrain.noiseScale = noiseScale;
            _terrain.noiseHeightMultiplier = noiseHeightMultiplier;
            _terrain.blurMap = blurMap;
            _terrain.blurRadius = blurRadius;
            _terrain.meshCompute = meshCompute;
            _terrain.densityCompute = densityCompute;
            _terrain.blurCompute = blurCompute;
            _terrain.editCompute = editCompute;
            _terrain.material = material;
            _terrain.OnGenerationEnd += PlaceObjects;

            Container.Bind<GenTest>().FromInstance(_terrain).AsSingle().NonLazy();
        }

        private void PlaceObjects()
        {
            PlaceRandom(stalagtitePrefab, stalagtiteCount);
            PlaceRandom(torpedoPrefab, torpedoesCount, true);
            
            _terrain.OnGenerationEnd -= PlaceObjects;
        }

        private void PlaceRandom(Object prefab, int count, bool normalOffset = false)
        {
            for (var i = 0; i < count; i++)
            {
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

                    var ind = Random.Range(0, hit.Length - 1);
                    if (hit.Length <= 3 || hit[ind].distance <= 0.1f ||
                        hit[ind].collider.gameObject.CompareTag("Submarine")) continue;

                    rh = hit[ind];
                    break;
                }

                Container.InstantiatePrefab(prefab, rh.point + (normalOffset ? rh.normal : Vector3.zero),
                    Quaternion.LookRotation(rh.normal),
                    null);
            }
        }
    }
}