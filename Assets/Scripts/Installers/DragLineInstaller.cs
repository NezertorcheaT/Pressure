using UnityEngine;
using Zenject;

namespace Installers
{
    public class DragLineInstaller : MonoInstaller
    {
        [SerializeField] private LineRenderer lr;
        [SerializeField] private Transform lineRenderLocation;

        public override void InstallBindings()
        {
            Container.Bind<DragLine>().FromInstance(new DragLine(lr, lineRenderLocation)).AsSingle().NonLazy();
        }
    }
}