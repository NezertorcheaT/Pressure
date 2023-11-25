using System.Linq;
using Controls;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Controls")]
    public class ControlsInstaller : MonoInstaller
    {
        [SerializeField] private ScriptableObject pc;
        [SerializeField] private ScriptableObject ps4;

        public override void InstallBindings()
        {
            ScriptableObject serv;

            serv = Input.GetJoystickNames().Contains("Wireless Controller") ? ps4 : pc;

            if (serv is not IControls controls)
                throw new System.NullReferenceException($"{serv.GetType().Name} does not realised from {nameof(IControls)}");

            Container.Bind<IControls>().FromInstance(controls).AsSingle().NonLazy();
            Debug.Log($"Controls ({controls.GetType().Name}) Installed");
        }
    }
}