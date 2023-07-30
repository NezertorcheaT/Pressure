using UnityEngine;
using UnityEngine.Events;

public class SubmarineCameras : MonoBehaviour
{
    [SerializeField] private Camera[] cameras;
    [SerializeField] private RenderTexture texture;

    private int _cameraId = 0;

    public Camera Current => cameras[_cameraId];

    public int CurrentId
    {
        get => _cameraId;
        private set
        {
            _cameraId = value;
            onChanged.Invoke();
        }
    }

    public UnityEvent onChanged;

    private void Update()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i != CurrentId)
            {
                cameras[i].gameObject.SetActive(false);
                cameras[i].targetTexture = null;
            }
        }

        cameras[CurrentId].gameObject.SetActive(true);
        cameras[CurrentId].targetTexture = texture;
    }

    public void NextCamera() => CurrentId = (int) Mathf.Repeat(_cameraId + 1, cameras.Length);
    public void PrevCamera() => CurrentId = (int) Mathf.Repeat(_cameraId - 1, cameras.Length);
}