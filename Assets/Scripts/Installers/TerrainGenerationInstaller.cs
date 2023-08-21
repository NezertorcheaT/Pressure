using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Installers
{
    [AddComponentMenu("Installers/Terrain")]
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

        [Space] [SerializeField] private GenerationBuildable[] generations;

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

            
        }

        private void PlaceObjects()
        {
            foreach (var buildable in generations)
            {
                for (var i = 0; i < buildable.count; i++)
                {
                    var gb = PlaceRandom(buildable.prefab, buildable.normalOffset, buildable.normalRotate);
                    if (buildable.prefab.Immerse)
                        ImmerseTerrain(gb);
                }
            }

            _terrain.OnGenerationEnd -= PlaceObjects;
            Container.Bind<GenTest>().FromInstance(_terrain).AsSingle().NonLazy();
        }

        private void ImmerseTerrain(GenerationBounds bounds)
        {
            foreach (var dot in bounds.Dots)
            {
                _terrain.Terraform(dot.Position, 15, dot.Diameter * 0.75f);
            }
        }

        private GenerationBounds PlaceRandom(GenerationBounds prefab, float normalOffset, bool normalRotate)
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

                var ind = Random.Range(1, hit.Length - 2);
                if (hit.Length <= 3 || hit[ind].distance <= 0.1f ||
                    hit[ind].collider.gameObject.CompareTag("Submarine")) continue;

                rh = hit[ind];
                break;
            }

            return Container.InstantiatePrefab(prefab, rh.point + rh.normal * normalOffset,
                normalRotate ? Quaternion.LookRotation(rh.normal) : Quaternion.identity,
                null).GetComponent<GenerationBounds>();
        }
    }
}