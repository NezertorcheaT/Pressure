using UnityEngine;
using Zenject;

public class DiscordRPCInstaller : MonoInstaller
{
    [SerializeField] private DRPC drpc;
    public override void InstallBindings()
    {
        drpc.Initialize();
        drpc.SetPresenceText("generating");
        Debug.Log("DiscordRPC Installed");
    }
}