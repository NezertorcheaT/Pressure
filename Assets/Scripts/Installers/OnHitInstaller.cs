using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/On Submarine Hit")]
    public class OnHitInstaller : MonoInstaller
    {
        [SerializeField] private SubmarineHit hit;

        public override void InstallBindings()
        {
            Container.Bind<SubmarineHit>().FromInstance(hit).AsSingle().NonLazy();
        }
    }
}