using UnityEngine;
using UnityEngine.Serialization;

public class CursorMover : MonoBehaviour
{
    [FormerlySerializedAs("CursorImage")] [SerializeField]
    private Transform cursorImage;

    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            cursorImage.transform.localPosition = new Vector3(0, 0, 0);
        else
            cursorImage.transform.position = Input.mousePosition;
    }
}