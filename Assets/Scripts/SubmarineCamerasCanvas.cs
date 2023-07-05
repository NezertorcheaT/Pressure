using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubmarineCamerasCanvas : MonoBehaviour
{
    [SerializeField] private SubmarineCameras submarineCameras;
    [SerializeField] private TextMeshProUGUI txt;

    private void Start()
    {
        submarineCameras.onChanged.AddListener(ChangeText);
        ChangeText();
    }
    private void ChangeText()
    {
        txt.text = submarineCameras.currentId.ToString();
    }
}
