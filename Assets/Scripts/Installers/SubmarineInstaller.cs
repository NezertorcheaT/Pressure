using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Submarine")]
    public class SubmarineInstaller : MonoInstaller
    {
        [SerializeField] private SubmarineMovement submarineMovement;
        [SerializeField] private Radar radar;
        [SerializeField] private SubmarineCameras submarineCameras;
        [SerializeField] private SubmarineHit hit;

        public override void InstallBindings()
        {
            Container.Bind<Submarine>().FromInstance(new Submarine(submarineMovement, radar, submarineCameras,hit))
                .AsSingle().NonLazy();
            Debug.Log("Submarine Installed");
        }
    }
}

public class Submarine
{
    public SubmarineMovement SubmarineMovement { get; set; }
    public Radar Radar { get; private set; }
    public SubmarineCameras SubmarineCameras { get; private set; }
    public SubmarineHit SubmarineHit { get; private set; }

    public Submarine(SubmarineMovement submarineMovement, Radar radar, SubmarineCameras submarineCameras, SubmarineHit submarineHit)
    {
        SubmarineMovement = submarineMovement;
        Radar = radar;
        SubmarineCameras = submarineCameras;
        SubmarineHit = submarineHit;
    }
}