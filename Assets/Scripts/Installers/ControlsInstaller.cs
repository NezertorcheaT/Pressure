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

            if (!(serv is IControls controls))
                throw new System.NullReferenceException();

            Container.Bind<IControls>().FromInstance(controls).AsSingle().NonLazy();
            Debug.Log("Controls Installed");
        }
    }
}