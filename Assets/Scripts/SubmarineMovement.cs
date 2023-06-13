using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SubmarineMovement : MonoBehaviour
{
    [SerializeField] private TwoSidePushTrigger LeftEngineLever;
    [SerializeField] private TwoSidePushTrigger RightEngineLever;
    [SerializeField] private Transform LeftEngine;
    [SerializeField] private Transform RightEngine;
    [SerializeField] private Transform Cabine;
    private float LeftEngineAngle;
    private float RightEngineAngle;
    [SerializeField] private TwoSidePushTrigger YLever;
    [SerializeField] private float YSpeed;
    [SerializeField] private float EngineSpeed;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private Transform Waterpas;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Cabine.position = new Vector3(transform.position.x, RightEngine.position.y, Cabine.position.z);
        if (RightEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up)
            RightEngineAngle += RotationSpeed;
        if (RightEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Down)
            RightEngineAngle -= RotationSpeed;
        RightEngine.localRotation = Quaternion.Euler(new Vector3(RightEngineAngle * 500, 90, -90));

        if (LeftEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up)
            LeftEngineAngle += RotationSpeed;
        if (LeftEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Down)
            LeftEngineAngle -= RotationSpeed;
        LeftEngine.localRotation = Quaternion.Euler(new Vector3(-LeftEngineAngle * 500, 90, -90));

        Waterpas.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }
    private void FixedUpdate()
    {
        if (RightEngineLever.CurrentSide != TwoSideMouseTrigger.Side.None)
        {
            rb.angularVelocity += Vector3.up * RotationSpeed * (RightEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up ? 1 : -1);
        }
        if (LeftEngineLever.CurrentSide != TwoSideMouseTrigger.Side.None)
        {
            rb.angularVelocity += Vector3.up * RotationSpeed * (LeftEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up ? -1 : 1);
        }

        if (RightEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up && LeftEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Up)
        {
            rb.velocity -= Cabine.forward * EngineSpeed;
        }
        if (RightEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Down && LeftEngineLever.CurrentSide == TwoSideMouseTrigger.Side.Down)
        {
            rb.velocity += Cabine.forward * EngineSpeed;
        }

        if (YLever.CurrentSide == TwoSideMouseTrigger.Side.Up)
        {
            rb.velocity += Vector3.up * YSpeed;
        }
        if (YLever.CurrentSide == TwoSideMouseTrigger.Side.Down)
        {
            rb.velocity -= Vector3.up * YSpeed;
        }
        Quaternion deltaQuat = Quaternion.FromToRotation(rb.transform.up, Vector3.up);

        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);

        float dampenFactor = 0.8f; // this value requires tuning
        rb.AddTorque(-rb.angularVelocity * dampenFactor, ForceMode.Acceleration);

        float adjustFactor = 0.1f; // this value requires tuning
        rb.AddTorque(axis.normalized * angle * adjustFactor, ForceMode.Acceleration);

        if (Mathf.Abs(rb.angularVelocity.y) > RotationSpeed*2)
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, rb.angularVelocity.z);
        if (Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2)) > EngineSpeed * 2)
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        if (Mathf.Abs(rb.velocity.y) > YSpeed)
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
}
