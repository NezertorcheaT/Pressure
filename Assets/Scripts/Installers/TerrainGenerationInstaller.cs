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

        public override void InstallBindings()
        {
            var genTest = Container.InstantiateComponentOnNewGameObject<GenTest>("Terrain");

            genTest.gameObject.layer = layer;
            genTest.numChunks = numChunks;
            genTest.numPointsPerAxis = numPointsPerAxis;
            genTest.boundsSize = boundsSize;
            genTest.isoLevel = isoLevel;
            genTest.useFlatShading = useFlatShading;
            genTest.noiseScale = noiseScale;
            genTest.noiseHeightMultiplier = noiseHeightMultiplier;
            genTest.blurMap = blurMap;
            genTest.blurRadius = blurRadius;
            genTest.meshCompute = meshCompute;
            genTest.densityCompute = densityCompute;
            genTest.blurCompute = blurCompute;
            genTest.editCompute = editCompute;
            genTest.material = material;

            Container.Bind<GenTest>().FromInstance(genTest).AsSingle().NonLazy();
        }
    }
}