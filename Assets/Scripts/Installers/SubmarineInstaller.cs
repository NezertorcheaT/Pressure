using UnityEngine;
using Zenject;

namespace Installers
{
    public class SubmarineInstaller : MonoInstaller
    {
        [SerializeField] private SubmarineMovement submarineMovement;
        [SerializeField] private Radar radar;
        [SerializeField] private SubmarineCameras submarineCameras;

        public override void InstallBindings()
        {
            Container.BindInstance(new Submarine(submarineMovement, radar, submarineCameras));
        }
    }
}

public class Submarine
{
    public SubmarineMovement SubmarineMovement { get; set; }
    public Radar Radar { get; private set; }
    public SubmarineCameras SubmarineCameras { get; private set; }

    public Submarine(SubmarineMovement submarineMovement, Radar radar, SubmarineCameras submarineCameras)
    {
        SubmarineMovement = submarineMovement;
        Radar = radar;
        SubmarineCameras = submarineCameras;
    }
}