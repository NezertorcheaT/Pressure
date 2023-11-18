using System;
using DiscordRPC;
using DiscordRPC.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DRPC : MonoBehaviour
{
    private DiscordRpcClient client;

    private RichPresence _presence;
    [SerializeField] private string startDetails = "Level 1";
    [SerializeField] private string startState = "Generating";

    public void SetPresenceText(string text)
    {
        _presence.State = text;
        client.SetPresence(_presence);
    }

    //Called when your application first starts.
    //For example, just before your main loop, on OnEnable for unity.
    public void Initialize()
    {
        _presence = new RichPresence()
        {
            Details = startDetails,
            State = startState,
            Assets = new Assets()
            {
                LargeImageKey = "https://cdn.discordapp.com/attachments/1065620552984821831/1172615797248643113/logo.png",
                LargeImageText = "NezertorcheaT's Pressure logo",
                SmallImageKey = "https://cdn.discordapp.com/attachments/1065620552984821831/1172614359386701915/logo_low.png",
            },
        };
        /*
        Create a Discord client
        NOTE:   If you are using Unity3D, you must use the full constructor and define
                 the pipe connection.
        */
        client = new DiscordRpcClient("1172608001841053719");

        //Set the logger
        client.Logger = new ConsoleLogger() {Level = LogLevel.Warning};

        //Subscribe to events
        client.OnReady += (sender, e) => { Console.WriteLine("Received Ready from user {0}", e.User.Username); };

        client.OnPresenceUpdate += (sender, e) => { Console.WriteLine("Received Update! {0}", e.Presence); };

        //Connect to the RPC
        client.Initialize();

        //Set the rich presence
        //Call this as many times as you want and anywhere in your code.
        client.SetPresence(_presence);
        SceneManager.sceneUnloaded += scene =>
        {
            if (scene == SceneManager.GetActiveScene()) Deinitialize();
        };
    }

    //The main loop of your application, or some sort of timer. Literally the Update function in Unity3D
    private void Update()
    {
        //Invoke all the events, such as OnPresenceUpdate
        client.Invoke();
    }

    //Called when your application terminates.
    //For example, just after your main loop, on OnDisable for unity.
    private void Deinitialize()
    {
        client.Dispose();
    }

    private void OnApplicationQuit()
    {
        Deinitialize();
    }
}