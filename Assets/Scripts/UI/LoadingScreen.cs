using Controls;
using Installers;
using TMPro;
using UnityEngine;
using Zenject;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GenerationInstaller tg;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI inputText;

    private IControls controls;

    [Inject]
    private void Construct(IControls controls)
    {
        this.controls = controls;
        tg.OnObjectPlaced += Change;
    }

    private void Start()
    {
        inputText.text = "Controls:\n" +
                         $"\"{controls.JumpKey.ShowName}\" is \"{controls.JumpKey.Key}\"\n" +
                         $"\"{controls.MousePos.ShowName}\" is \"{controls.MousePos.Key}\"\n" +
                         $"\"{controls.UseKey.ShowName}\" is \"{controls.UseKey.Key}\"\n" +
                         $"\"{controls.EscKeyUp.ShowName}\" is \"{controls.EscKeyUp.Key}\"\n" +
                         $"\"{controls.FlashLightKey.ShowName}\" is \"{controls.FlashLightKey.Key}\"\n" +
                         $"\"{controls.ItemUseKey.ShowName}\" is \"{controls.ItemUseKey.Key}\"\n" +
                         $"\"{controls.MouseButtonUp.ShowName}\" is \"{controls.MouseButtonUp.Key}\"\n" +
                         $"\"{controls.MouseScrollWheel.ShowName}\" is \"{controls.MouseScrollWheel.Key}\"\n" +
                         $"\"{controls.WASD.ShowName}\" is \"{controls.WASD.Key}\"";
    }

    private void Change(long ms,int iter)
    {
        text.text = $"{(int) ((float) iter / (float) tg.MaxIterations * 100f)}%\nGeneration Time: {ms} ms";
    }
}