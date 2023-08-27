using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
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
            _terrain.OnGenerationEnd += GenerationEnded;
            _terrain.Initialize();
        }

        private void GenerationEnded()
        {
            Debug.Log("Terrain Installed");
            Container.Bind<GenTest>().FromInstance(_terrain).AsSingle().NonLazy();
        }
    }
}