using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class SubmarineMovement : MonoBehaviour
{
    [FormerlySerializedAs("LeftEngineLever")] [SerializeField]
    private TwoSidePushTrigger leftEngineLever;

    [FormerlySerializedAs("RightEngineLever")] [SerializeField]
    private TwoSidePushTrigger rightEngineLever;

    [FormerlySerializedAs("YLever")] [SerializeField]
    private TwoSidePushTrigger yLever;

    [FormerlySerializedAs("LeftEngine")] [SerializeField]
    private Transform leftEngine;

    [FormerlySerializedAs("RightEngine")] [SerializeField]
    private Transform rightEngine;

    [FormerlySerializedAs("Cabine")] [SerializeField]
    private Transform cabine;

    [FormerlySerializedAs("YSpeed")] [SerializeField]
    private float ySpeed;

    [FormerlySerializedAs("EngineSpeed")] [SerializeField]
    private float engineSpeed;

    [FormerlySerializedAs("RotationSpeed")] [SerializeField]
    private float rotationSpeed;

    [FormerlySerializedAs("MaxFuel")] [SerializeField]
    private float maxFuel = 1000;

    [FormerlySerializedAs("FuelReduceMultiplier")] [SerializeField]
    private float fuelReduceMultiplier = 0.01f;

    [FormerlySerializedAs("MaxAir")] [SerializeField]
    private float maxAir = 1000;

    [FormerlySerializedAs("AirReduceSpeed")] [SerializeField]
    private float airReduceSpeed = 0.01f;

    [FormerlySerializedAs("AirCanvas")] [SerializeField]
    private PressureCanvas airCanvas;

    [FormerlySerializedAs("YCanvas")] [SerializeField]
    private PressureCanvas yCanvas;

    [FormerlySerializedAs("GoYCanvas")] [SerializeField]
    private PressureCanvas goYCanvas;

    [SerializeField] private TextMeshProUGUI fuelText;

    [FormerlySerializedAs("Waterpas")] [SerializeField]
    private Transform waterpas;

    private Rigidbody _rb;
    private float _leftEngineAngle;
    private float _rightEngineAngle;
    private float _fuel;
    private float _air;

    private float Fuel
    {
        get => Mathf.Clamp(_fuel, 0, maxFuel);
        set
        {
            _fuel = Mathf.Clamp(value, 0, maxFuel);
            fuelText.text = Mathf.Round(Fuel).ToString() + " l";
        }
    }

    private float Air
    {
        get => Mathf.Clamp(_air, 0, maxAir);
        set
        {
            _air = Mathf.Clamp(value, 0, maxAir);
            airCanvas.Pressure = Air / maxAir;
        }
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Fuel = maxFuel;
        Air = maxAir;

        goYCanvas.Pressure = 0.5f;
        airCanvas.Pressure = Air / maxAir;
        yCanvas.Pressure = (transform.position.y + 500f) / 1000f;

        yLever.onSideUp.AddListener(() => goYCanvas.Pressure = 0);
        yLever.onSideNone.AddListener(() => goYCanvas.Pressure = 0.5f);
        yLever.onSideDown.AddListener(() => goYCanvas.Pressure = 1);
    }

    private void Update()
    {
        Air -= airReduceSpeed * Time.deltaTime;
        cabine.position = new Vector3(transform.position.x, rightEngine.position.y, cabine.position.z);

        if (Fuel != 0)
        {
            if (rightEngineLever.currentSide == TwoSideMouseTrigger.Side.Up)
            {
                _rightEngineAngle += rotationSpeed;
            }

            if (rightEngineLever.currentSide == TwoSideMouseTrigger.Side.Down)
            {
                _rightEngineAngle -= rotationSpeed;
            }

            rightEngine.localRotation = Quaternion.Euler(new Vector3(_rightEngineAngle * 500, 90, -90));

            if (leftEngineLever.currentSide == TwoSideMouseTrigger.Side.Up)
            {
                _leftEngineAngle += rotationSpeed;
            }

            if (leftEngineLever.currentSide == TwoSideMouseTrigger.Side.Down)
            {
                _leftEngineAngle -= rotationSpeed;
            }

            leftEngine.localRotation = Quaternion.Euler(new Vector3(-_leftEngineAngle * 500, 90, -90));
        }

        waterpas.eulerAngles = -transform.eulerAngles;
    }

    private void FixedUpdate()
    {
        yCanvas.Pressure = 1f - ((transform.position.y + 500f) / 1000f);
        if (Fuel != 0)
        {
            if (rightEngineLever.currentSide != TwoSideMouseTrigger.Side.None)
            {
                _rb.angularVelocity += Vector3.up *
                                       (rotationSpeed * (rightEngineLever.currentSide == TwoSideMouseTrigger.Side.Up
                                           ? 1
                                           : -1));
                Fuel -= rotationSpeed * fuelReduceMultiplier;
            }

            if (leftEngineLever.currentSide != TwoSideMouseTrigger.Side.None)
            {
                _rb.angularVelocity += Vector3.up *
                                       (rotationSpeed * (leftEngineLever.currentSide == TwoSideMouseTrigger.Side.Up
                                           ? -1
                                           : 1));
                Fuel -= rotationSpeed * fuelReduceMultiplier;
            }

            if (rightEngineLever.currentSide == TwoSideMouseTrigger.Side.Up &&
                leftEngineLever.currentSide == TwoSideMouseTrigger.Side.Up)
            {
                _rb.velocity -= cabine.forward * engineSpeed;
                Fuel -= engineSpeed * fuelReduceMultiplier;
            }

            if (rightEngineLever.currentSide == TwoSideMouseTrigger.Side.Down &&
                leftEngineLever.currentSide == TwoSideMouseTrigger.Side.Down)
            {
                _rb.velocity += cabine.forward * engineSpeed;
                Fuel -= engineSpeed * fuelReduceMultiplier;
            }

            if (yLever.currentSide == TwoSideMouseTrigger.Side.Up)
            {
                _rb.velocity += Vector3.up * ySpeed;
                Fuel -= ySpeed * fuelReduceMultiplier;
            }

            if (yLever.currentSide == TwoSideMouseTrigger.Side.Down)
            {
                _rb.velocity -= Vector3.up * ySpeed;
                Fuel -= ySpeed * fuelReduceMultiplier;
            }
        }

        Quaternion deltaQuat = Quaternion.FromToRotation(_rb.transform.up, Vector3.up);

        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);

        float dampenFactor = 0.8f; // this value requires tuning
        _rb.AddTorque(-_rb.angularVelocity * dampenFactor, ForceMode.Acceleration);

        float adjustFactor = 0.1f; // this value requires tuning
        _rb.AddTorque(axis.normalized * (angle * adjustFactor), ForceMode.Acceleration);

        if (Mathf.Abs(_rb.angularVelocity.y) > rotationSpeed * 2)
            _rb.angularVelocity = new Vector3(_rb.angularVelocity.x, 0, _rb.angularVelocity.z);
        if (Mathf.Sqrt(Mathf.Pow(_rb.velocity.x, 2) + Mathf.Pow(_rb.velocity.z, 2)) > engineSpeed * 2)
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        if (Mathf.Abs(_rb.velocity.y) > ySpeed)
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
    }
}