using System;
using System.Threading.Tasks;
using Generation;
using Generation.Placers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Random = UnityEngine.Random;

namespace Installers
{
    [AddComponentMenu("Installers/Generation")]
    public class GenerationInstaller : MonoInstaller
    {
        public event Action<long, int> OnObjectPlaced;
        public int MaxIterations { get; private set; }
        [Space] [SerializeField] private GenerationBuildable[] generations;
        [SerializeField] private bool generate = true;
        [Space] [SerializeField] private UnityEvent allGenerationEnded;

        private System.Diagnostics.Stopwatch ObjectPlacementTimer;
        private GenTest _terrain;
        private int _iter;

        public override void InstallBindings()
        {
            _terrain = FindObjectOfType<GenTest>();
            PlaceObjects();
        }

        private async Task PlaceBuildable(GenerationBuildable buildable)
        {
            for (var i = 0; i < buildable.count; i++)
            {
                await ((IGenerationPlacer) buildable.placer).PlaceRandom(buildable, ImmerseTerrain,
                    Container.InstantiatePrefab,
                    Mathf.Sqrt(232.4562f * 232.4562f + 1.970166f * 1.970166f + 16.56459f * 16.56459f) * 2f);
                _iter++;
                OnObjectPlaced?.Invoke(ObjectPlacementTimer.ElapsedMilliseconds, _iter);
            }
        }

        private async Task PlaceObjects()
        {
            MaxIterations = 0;
            foreach (var buildable in generations)
            {
                if (buildable.process)
                    MaxIterations += buildable.count;
            }

            ObjectPlacementTimer = new System.Diagnostics.Stopwatch();
            ObjectPlacementTimer.Start();
            if (generate)
            {
                foreach (var buildable in generations)
                {
                    if (!buildable.process) continue;
                    PlaceBuildable(buildable);
                }
            }

            for (;;)
            {
                if (_iter != MaxIterations)
                    await Task.Delay(500);
                else
                    break;
            }

            allGenerationEnded.Invoke();
            ObjectPlacementTimer.Stop();

            //Debug.Log("Generation Installed.\n Object placement time: " + ObjectPlacementTimer.ElapsedMilliseconds + " ms");
            Container.Bind();
        }

        private void ImmerseTerrain(GenerationBounds bounds)
        {
            foreach (var dot in bounds.Dots)
            {
                _terrain.Terraform(dot.Position, 15, dot.Diameter * 0.75f);
            }
        }
    }
}