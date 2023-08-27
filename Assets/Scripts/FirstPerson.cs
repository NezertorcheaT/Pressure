using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

    [FormerlySerializedAs("FlashLight")] [SerializeField]
    private Light flashLight;

    private Rigidbody _rb;
    private bool _isCursorFree = true;

    public bool IsCursorFree
    {
        get => _isCursorFree;
        set => _isCursorFree = value;
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
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (IsWorking && isUnderWater && Input.GetKeyUp(KeyCode.F)) ToggleFlashLight();
    }

    private void FixedUpdate()
    {
        InternalLockUpdate();
        Cursor.visible = _isCursorFree;
        Cursor.lockState =
            _isCursorFree ? CursorLockMode.None : (IsWorking ? CursorLockMode.Locked : CursorLockMode.None);

        if (!IsWorking)
        {
            _isCursorFree = false;
            _rb.velocity = Vector3.zero;
            return;
        }


        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        var inputX = Input.GetAxisRaw("Horizontal");
        var inputY = Input.GetAxisRaw("Vertical");
        var inputJump = Input.GetKey(KeyCode.Space).x();

        var velocity = (transform.right * inputX + transform.forward * inputY).normalized * speed +
                       isUnderWater.x() * inputJump * jumpForce * Vector3.up;

        var rotation = new Vector3(0, Input.GetAxisRaw("Mouse X"), 0) * sensitivity;

        var cameraRotation = new Vector3(Input.GetAxisRaw("Mouse Y"), 0, 0) * sensitivity;


        _rb.velocity = velocity - isUnderWater.x() * gravity * Vector3.up;

        if (rotation != Vector3.zero)
        {
            _rb.MoveRotation(_rb.rotation * Quaternion.Euler(rotation));
        }

        if (cam)
        {
            cam.transform.Rotate(-cameraRotation);
        }
    }

    public void ToggleFlashLight() => flashLight.gameObject.SetActive(!flashLight.gameObject.activeSelf);
    public void OffFlashLight() => flashLight.gameObject.SetActive(false);
    public void OnFlashLight() => flashLight.gameObject.SetActive(true);

    public void Teleport(Transform target)
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _isCursorFree = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isCursorFree = false;
        }
    }
}