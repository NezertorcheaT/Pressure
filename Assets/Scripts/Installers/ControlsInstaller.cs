using System.Linq;
using Controls;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Controls")]
    public class ControlsInstaller : MonoInstaller
    {
        [SerializeField] private ScriptableObject pc;
        [SerializeField] private ScriptableObject ps4;
        [SerializeField] private GameObject logitech;

        public override void InstallBindings()
        {
            ScriptableObject serv;

            logitech?.SetActive(false);
            if (Input.GetJoystickNames().Contains("Wireless Controller"))
            {
                logitech?.SetActive(true);
                serv = ps4;
            }
            else
            {
                serv = pc;
            }

            if (!(serv is IControls controls))
                throw new System.NullReferenceException(
                    $"{serv.GetType().Name} does not realised from {nameof(IControls)}");

            Container.Bind<IControls>().FromInstance(controls).AsSingle().NonLazy();
            Debug.Log($"Controls ({controls.GetType().Name}) Installed");
        }
    }
}