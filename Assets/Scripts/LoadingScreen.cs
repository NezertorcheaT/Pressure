using Installers;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TerrainGenerationInstaller tg;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        tg.OnObjectPlaced += Change;
    }

    private void Change(long ms)
    {
        text.text = "Generation Time: " + ms + " ms";
    }
}