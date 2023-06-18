using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPerson : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float sensitivity = 3.0f;
    [SerializeField, Range(0f, 90f)] private float clampAngle = 60f;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform camOrigin;
    [SerializeField] private bool isWorking = false;
    public bool IsWorking
    {
        get => isWorking;
        set => isWorking = value;
    }
    public Transform CamOrigin => camOrigin;

    private Rigidbody rb;
    private bool isCursorFree = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (IsWorking)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");

            Vector3 velocity = (transform.right * inputX + transform.forward * inputY).normalized * speed;

            Vector3 rotation = new Vector3(0, Input.GetAxisRaw("Mouse X"), 0) * sensitivity;

            Vector3 cameraRotation = new Vector3(Input.GetAxisRaw("Mouse Y"), 0, 0) * sensitivity;


            rb.velocity = velocity;

            if (rotation != Vector3.zero)
            {
                rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
            }

            if (cam != null)
            {
                cam.transform.Rotate(-cameraRotation);
                //cam.transform.eulerAngles-=cameraRotation;
                //cam.transform.eulerAngles = new Vector3(Mathf.Clamp(cam.transform.eulerAngles.x, 90, 270), cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
            }

            InternalLockUpdate();

            if (isCursorFree)
            {
                UnlockCursor();
            }
            else if (!isCursorFree)
            {
                LockCursor();
            }
        }
        else
        {
            LockCursor();
        }
    }
    public void Teleport(Transform target)
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isCursorFree = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isCursorFree = true;
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
