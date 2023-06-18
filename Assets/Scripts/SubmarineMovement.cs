using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class SubmarineMovement : MonoBehaviour
{
    [SerializeField] private TwoSidePushTrigger LeftEngineLever;
    [SerializeField] private TwoSidePushTrigger RightEngineLever;
    [SerializeField] private TwoSidePushTrigger YLever;

    [SerializeField] private Transform LeftEngine;
    [SerializeField] private Transform RightEngine;
    [SerializeField] private Transform Cabine;

    [SerializeField] private float YSpeed;
    [SerializeField] private float EngineSpeed;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float MaxFuel = 1000;
    [SerializeField] private float FuelReduceMultiplier = 0.01f;
    [SerializeField] private float MaxAir = 1000;
    [SerializeField] private float AirReduceSpeed = 0.01f;

    [SerializeField] private PressureCanvas AirCanvas;
    [SerializeField] private PressureCanvas YCanvas;
    [SerializeField] private PressureCanvas GoYCanvas;

    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private Transform Waterpas;

    private Rigidbody rb;
    private float LeftEngineAngle;
    private float RightEngineAngle;
    private float fuel;
    private float air;

    private float Fuel
    {
        get => Mathf.Clamp(fuel, 0, MaxFuel);
        set
        {
            fuel = Mathf.Clamp(value, 0, MaxFuel);
            fuelText.text = Mathf.Round(Fuel).ToString() + " l";
        }
    }
    private float Air
    {
        get => Mathf.Clamp(air, 0, MaxAir);
        set
        {
            air = Mathf.Clamp(value, 0, MaxAir);
            AirCanvas.Pressure = Air / MaxAir;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Fuel = MaxFuel;
        Air = MaxAir;

        GoYCanvas.Pressure = 0.5f;
        AirCanvas.Pressure = Air / MaxAir;
        YCanvas.Pressure = (transform.position.y + 500f) / 1000f;

        YLever.onSideUp.AddListener(() => GoYCanvas.Pressure = 0);
        YLever.onSideNone.AddListener(() => GoYCanvas.Pressure = 0.5f);
        YLever.onSideDown.AddListener(() => GoYCanvas.Pressure = 1);

    }
    private void Update()
    {
        Air -= AirReduceSpeed * Time.deltaTime;
        Cabine.position = new Vector3(transform.position.x, RightEngine.position.y, Cabine.position.z);

        if (Fuel != 0)
        {
            if (RightEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up)
            {
                RightEngineAngle += RotationSpeed;
            }
            if (RightEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Down)
            {
                RightEngineAngle -= RotationSpeed;
            }
            RightEngine.localRotation = Quaternion.Euler(new Vector3(RightEngineAngle * 500, 90, -90));

            if (LeftEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up)
            {
                LeftEngineAngle += RotationSpeed;
            }
            if (LeftEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Down)
            {
                LeftEngineAngle -= RotationSpeed;
            }
            LeftEngine.localRotation = Quaternion.Euler(new Vector3(-LeftEngineAngle * 500, 90, -90));
        }

        Waterpas.eulerAngles = -transform.eulerAngles;
    }
    private void FixedUpdate()
    {
        YCanvas.Pressure = (transform.position.y + 500f) / 1000f;
        if (Fuel != 0)
        {
            if (RightEngineLever.CurrentSide != TwoSideMouseTrigger.Side.None)
            {
                rb.angularVelocity += Vector3.up * RotationSpeed * (RightEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up ? 1 : -1);
                Fuel -= RotationSpeed * FuelReduceMultiplier;
            }
            if (LeftEngineLever.CurrentSide != TwoSideMouseTrigger.Side.None)
            {
                rb.angularVelocity += Vector3.up * RotationSpeed * (LeftEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up ? -1 : 1);
                Fuel -= RotationSpeed * FuelReduceMultiplier;
            }

            if (RightEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up && LeftEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up)
            {
                rb.velocity -= Cabine.forward * EngineSpeed;
                Fuel -= EngineSpeed * FuelReduceMultiplier;
            }
            if (RightEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Down && LeftEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Down)
            {
                rb.velocity += Cabine.forward * EngineSpeed;
                Fuel -= EngineSpeed * FuelReduceMultiplier;
            }

            if (YLever.CurrentSide == TwoSideMouseTrigger.Side.Up)
            {
                rb.velocity += Vector3.up * YSpeed;
                Fuel -= YSpeed * FuelReduceMultiplier;
            }
            if (YLever.CurrentSide == TwoSideMouseTrigger.Side.Down)
            {
                rb.velocity -= Vector3.up * YSpeed;
                Fuel -= YSpeed * FuelReduceMultiplier;
            }
        }
        Quaternion deltaQuat = Quaternion.FromToRotation(rb.transform.up, Vector3.up);

        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);

        float dampenFactor = 0.8f; // this value requires tuning
        rb.AddTorque(-rb.angularVelocity * dampenFactor, ForceMode.Acceleration);

        float adjustFactor = 0.1f; // this value requires tuning
        rb.AddTorque(axis.normalized * angle * adjustFactor, ForceMode.Acceleration);

        if (Mathf.Abs(rb.angularVelocity.y) > RotationSpeed * 2)
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, rb.angularVelocity.z);
        if (Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2)) > EngineSpeed * 2)
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        if (Mathf.Abs(rb.velocity.y) > YSpeed)
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
}
