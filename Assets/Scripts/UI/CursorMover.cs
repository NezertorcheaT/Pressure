using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class CursorMover : MonoBehaviour
{
    [FormerlySerializedAs("CursorImage")] [SerializeField]
    private Transform cursorImage;

    private IControls controls;
    [Inject]
    private void Construct(IControls controls)
    {
        this.controls = controls;
    }
    
    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            cursorImage.transform.localPosition = new Vector3(0, 0, 0);
        else
            cursorImage.transform.position = controls.MousePos;
    }
}