using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Boids")]
    public class BoidsInstaller : MonoInstaller
    {
        [SerializeField] private BoidManager managerPrefab;

        public override void InstallBindings()
        {
            var manager = Container.InstantiatePrefabForComponent<BoidManager>(managerPrefab);
            Debug.Log("BoidManager Installed");
            Container.Bind<BoidManager>().FromInstance(manager).AsSingle().NonLazy();
        }
    }
}