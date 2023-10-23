using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Player")]
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private FirstPerson player;
        [SerializeField] private ItemsShow itemsShow;

        public override void InstallBindings()
        {
            Debug.Log("Player Installed");
            Container.Bind<FirstPerson>().FromInstance(player).AsSingle().NonLazy();
            Container.Bind<ItemsShow>().FromInstance(itemsShow).AsSingle().NonLazy();
        }
    }
}