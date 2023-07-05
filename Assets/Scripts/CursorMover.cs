using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMover : MonoBehaviour
{
    [SerializeField] private Transform CursorImage;

    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            CursorImage.transform.localPosition = new Vector3(0, 0, 0);
        else
            CursorImage.transform.position = Input.mousePosition;
    }
}
