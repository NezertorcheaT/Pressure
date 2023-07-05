using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubmarineCameras : MonoBehaviour
{
    [SerializeField]
    private Camera[] cameras;
    private int cameraId = 0;
    [SerializeField]
    private RenderTexture texture;

    public Camera current => cameras[cameraId];
    public int currentId
    {
        get => cameraId;
        private set
        {
            cameraId = value;
            onChanged.Invoke();
        }
    }
    public UnityEvent onChanged;

    private void Update()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i != currentId)
            {
                cameras[i].gameObject.SetActive(false);
                cameras[i].targetTexture = null;
            }
        }
        cameras[currentId].gameObject.SetActive(true);
        cameras[currentId].targetTexture = texture;
    }
    public void NextCamera() => currentId = (int)Mathf.Repeat(cameraId + 1, cameras.Length);
    public void PrevCamera() => currentId = (int)Mathf.Repeat(cameraId - 1, cameras.Length);
}
