using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Controls")]
    public class ControlsInstaller : MonoInstaller
    {
        [SerializeField] private ScriptableObject obj;

        public override void InstallBindings()
        {
            if (!(obj is IControls controls))
                throw new System.NullReferenceException();
            
            Container.Bind<IControls>().FromInstance(controls).AsSingle().NonLazy();
            Debug.Log("Controls Installed");
        }
    }
}