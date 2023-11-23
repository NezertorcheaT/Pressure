using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class FirstPerson : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField, Min(0)] private float gravity = 0.1f;
    [SerializeField, Min(0)] private float jumpForce = 1f;
    [SerializeField] private float sensitivity = 3.0f;
    [SerializeField, Range(0f, 90f)] private float clampAngle = 80f;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform camOrigin;
    [SerializeField] private bool isWorking = false;
    [SerializeField] private bool isUnderWater = false;

    [FormerlySerializedAs("FlashLight")] [SerializeField]
    private Light flashLight;

    private Rigidbody _rb;
    private float _xRotaion = 0;
    private bool _isCursorFree = true;

    public bool CursorLocked => !(IsCursorFree && !IsWorking);
    public bool IsCursorFree
    {
        get => _isCursorFree;
        set => _isCursorFree = value;
    }

    public bool IsWorking
    {
        get => isWorking;
        set
        {
            isWorking = value;
            _xRotaion = Mathf.Clamp(cam.transform.localRotation.eulerAngles.x.NormalizeAngle(), -clampAngle, clampAngle);
        }
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
            _xRotaion = Mathf.Clamp(cam.transform.localRotation.eulerAngles.x.NormalizeAngle(), -clampAngle, clampAngle);
            return;
        }


        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        var inputX = Input.GetAxisRaw("Horizontal");
        var inputY = Input.GetAxisRaw("Vertical");
        var inputJump = Input.GetKey(KeyCode.Space).x();

        Vector3 velocity;
        if (IsUnderWater)
        {
            velocity = (transform.right * inputX + cam.transform.forward * inputY).normalized * speed +
                       inputJump * jumpForce * Vector3.up;
        }
        else
        {
            velocity = (transform.right * inputX + transform.forward * inputY).normalized * speed;
        }

        var rotation = new Vector3(0, Input.GetAxisRaw("Mouse X"), 0) * sensitivity;

        _xRotaion -= Input.GetAxisRaw("Mouse Y") * sensitivity;
        _xRotaion = Mathf.Clamp(_xRotaion, -clampAngle, clampAngle);
        //_xRotaion = Mathf.Clamp(_xRotaion.NormalizeAngle(), -clampAngle, clampAngle);
        //_xRotaion = (_xRotaion).ModularClamp(-clampAngle, clampAngle);

        _rb.velocity = velocity - gravity * Vector3.up;

        if (rotation != Vector3.zero)
        {
            _rb.MoveRotation(_rb.rotation * Quaternion.Euler(rotation));
        }

        if (cam)
        {
            cam.transform.localRotation = Quaternion.Euler(_xRotaion, 0, 0);
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