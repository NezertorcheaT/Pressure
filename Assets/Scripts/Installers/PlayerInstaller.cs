using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Player")]
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private FirstPerson player;

        public override void InstallBindings()
        {
            Container.Bind<FirstPerson>().FromInstance(player).AsSingle().NonLazy();
        }
    }
}