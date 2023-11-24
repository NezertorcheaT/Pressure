using Installers;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GenerationInstaller tg;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        tg.OnObjectPlaced += Change;
    }

    private void Change(long ms, int iter)
    {
        text.text = $"{(int) ((float) iter / (float) tg.MaxIterations * 100f)}%\nGeneration Time: {ms} ms";
    }
}