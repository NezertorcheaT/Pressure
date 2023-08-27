using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/DragLine")]
    public class DragLineInstaller : MonoInstaller
    {
        [SerializeField] private LineRenderer lr;
        [SerializeField] private Transform lineRenderLocation;

        public override void InstallBindings()
        {
            Debug.Log("DragLine Installed");
            Container.Bind<DragLine>().FromInstance(new DragLine(lr, lineRenderLocation)).AsSingle().NonLazy();
        }
    }
}