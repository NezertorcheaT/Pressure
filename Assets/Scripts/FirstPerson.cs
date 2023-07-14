using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPerson : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField, Min(0)] private float gravity = 0.1f;
    [SerializeField, Min(0)] private float jumpForce = 1f;
    [SerializeField] private float sensitivity = 3.0f;
    [SerializeField, Range(0f, 90f)] private float clampAngle = 60f;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform camOrigin;
    [SerializeField] private bool isWorking = false;
    [SerializeField] private bool isUnderWater = false;
    [SerializeField] private Light FlashLight;

    private Rigidbody rb;
    private bool isCursorFree = true;
    public bool IsCursorFree
    {
        get => isCursorFree;
        set => isCursorFree = value;
    }

    public bool IsWorking
    {
        get => isWorking;
        set => isWorking = value;
    }
    public bool IsUnderWater
    {
        get => isUnderWater;
        set => isUnderWater = value;
    }
    public Transform CamOrigin => camOrigin;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        InternalLockUpdate();
        Cursor.visible = isCursorFree;
        Cursor.lockState = isCursorFree ? CursorLockMode.None : (IsWorking ? CursorLockMode.Locked : CursorLockMode.None);

        if (!IsWorking)
        {
            isCursorFree = false;
            rb.velocity = Vector3.zero;
            return;
        }

        if (!isUnderWater)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
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
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.F))
                ToggleFlashLight();

            rb.constraints = RigidbodyConstraints.FreezeRotation;
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            float inputJump = Input.GetKey(KeyCode.Space) ? 1 : 0;

            Vector3 velocity = (transform.right * inputX + transform.forward * inputY).normalized * speed + inputJump * jumpForce * Vector3.up;

            Vector3 rotation = new Vector3(0, Input.GetAxisRaw("Mouse X"), 0) * sensitivity;

            Vector3 cameraRotation = new Vector3(Input.GetAxisRaw("Mouse Y"), 0, 0) * sensitivity;



            rb.velocity = velocity - Vector3.up * gravity;

            if (rotation != Vector3.zero)
            {
                rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
            }

            if (cam != null)
            {
                cam.transform.Rotate(-cameraRotation);
            }
        }
    }

    public void ToggleFlashLight() => FlashLight.gameObject.SetActive(!FlashLight.gameObject.activeSelf);
    public void OffFlashLight() => FlashLight.gameObject.SetActive(false);
    public void OnFlashLight() => FlashLight.gameObject.SetActive(true);

    public void Teleport(Transform target)
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isCursorFree = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isCursorFree = false;
        }
    }
}
