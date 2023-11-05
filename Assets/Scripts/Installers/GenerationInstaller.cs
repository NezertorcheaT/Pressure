using System;
using System.Threading.Tasks;
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
        private int _endedChunks = 0;

        private System.Diagnostics.Stopwatch ObjectPlacementTimer;
        private GenTest _terrain;

        public override void InstallBindings()
        {
            _terrain = FindObjectOfType<GenTest>();
            PlaceObjects();
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
                var iter = 0;
                foreach (var buildable in generations)
                {
                    if (!buildable.process) continue;
                    for (var i = 0; i < buildable.count; i++)
                    {
                        iter++;
                        await PlaceRandom(buildable);
                        OnObjectPlaced(ObjectPlacementTimer.ElapsedMilliseconds, iter);
                    }
                }
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

        private async Task PlaceRandom(GenerationBuildable buildable)
        {
            await Task.Delay(1);

            var boundsSize = _terrain.boundsSize;
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

            var gb = Container.InstantiatePrefab(buildable.prefab, rh.point + rh.normal * buildable.normalOffset,
                buildable.normalRotate ? Quaternion.LookRotation(rh.normal) : Quaternion.identity,
                null).GetComponent<GenerationBounds>();

            if (buildable.prefab.Immerse) ImmerseTerrain(gb);
        }
    }
}