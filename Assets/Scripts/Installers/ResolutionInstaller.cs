using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Resolution")]
    public class ResolutionInstaller : MonoInstaller
    {
        [SerializeField] private Vector2Int resolution;

        public override void InstallBindings()
        {
            //Screen.SetResolution(640, 480, FullScreenMode.ExclusiveFullScreen, new RefreshRate() { numerator = 60, denominator = 1 });
            Screen.SetResolution(resolution.x, resolution.y, true);
            Debug.Log("Resolution Installed");
        }
    }
}