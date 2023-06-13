using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineCameras : MonoBehaviour
{
    [SerializeField]
    private Camera[] cameras;
    private int cameraId = 0;
    [SerializeField]
    private RenderTexture texture;

    public Camera current => cameras[cameraId];

    private void Update()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i != cameraId)
            {
                cameras[i].gameObject.SetActive(false);
                cameras[i].targetTexture = null;
            }
        }
        cameras[cameraId].gameObject.SetActive(true);
        cameras[cameraId].targetTexture = texture;
    }
    public void NextCamera() => cameraId = (int)Mathf.Repeat(cameraId + 1, cameras.Length);
    public void PrevCamera() => cameraId = (int)Mathf.Repeat(cameraId - 1, cameras.Length);
}
