using Controls;
using UnityEngine;
using Zenject;

// Original script created by https://youtube.com/c/Boxply

[RequireComponent(typeof(Rigidbody))]
public class DragRigidbody : MonoBehaviour
{
    [SerializeField] private float force = 600;
    [SerializeField] private float damping = 6;
    [SerializeField] private float distance = 15;

    private DragLine _dragLine;
    private ConfigurableJoint _joint;
    private float _dragDepth;
    private Transform _hitTransform;
    private IControls _controls;

    [Inject]
    private void Construct(DragLine dragLine, IControls controls)
    {
        _dragLine = dragLine;
        _controls = controls;
    }

    private void Update()
    {
        if (_controls.UseKey.Input) HandleInput(_controls.MousePos.Input);
        if (_controls.UseKeyDown.Input) HandleInputBegin(_controls.MousePos.Input);
        if (_controls.UseKeyUp.Input) HandleInputEnd();
    }

    public void HandleInputBegin(Vector3 screenPosition)
    {
        var ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("DragAndDrop"))
            {
                var go = new GameObject("Drag Hit Point");
                go.hideFlags = HideFlags.HideInHierarchy;
                go.transform.position = hit.point;
                go.transform.SetParent(hit.rigidbody.transform);
                _hitTransform = go.transform;

                _dragDepth = CameraPlane.CameraToPointDepth(Camera.main, hit.point);
                _joint = AttachJoint(hit.rigidbody, hit.point);
            }
        }

        _dragLine.LineRenderer.positionCount = 2;
    }

    public void HandleInput(Vector3 screenPosition)
    {
        if (_joint == null)
            return;
        _joint.transform.position = CameraPlane.ScreenToWorldPlanePoint(Camera.main, _dragDepth, screenPosition);

        DrawRope();
    }

    public void HandleInputEnd()
    {
        DestroyRope();
        if (_hitTransform)
        {
            Destroy(_hitTransform.gameObject);
            _hitTransform = null;
        }

        if (_joint)
            Destroy(_joint.gameObject);
    }

    ConfigurableJoint AttachJoint(Rigidbody rb, Vector3 attachmentPosition)
    {
        var go = new GameObject("Attachment Point");
        go.hideFlags = HideFlags.HideInHierarchy;
        go.transform.position = attachmentPosition;

        var newRb = go.AddComponent<Rigidbody>();
        newRb.isKinematic = true;

        var joint = go.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rb;
        joint.configuredInWorldSpace = true;
        joint.xDrive = NewJointDrive(force, damping);
        joint.yDrive = NewJointDrive(force, damping);
        joint.zDrive = NewJointDrive(force, damping);
        joint.slerpDrive = NewJointDrive(force, damping);
        joint.rotationDriveMode = RotationDriveMode.Slerp;

        return joint;
    }

    private static JointDrive NewJointDrive(float force, float damping)
    {
        var drive = new JointDrive();
        drive.mode = JointDriveMode.Position;
        drive.positionSpring = force;
        drive.positionDamper = damping;
        drive.maximumForce = Mathf.Infinity;
        return drive;
    }

    private void DrawRope()
    {
        if (_joint == null) return;

        _dragLine.LineRenderer.SetPosition(0, _dragLine.LineRenderLocation.position);
        _dragLine.LineRenderer.SetPosition(1, _hitTransform.position);
    }

    private void DestroyRope()
    {
        if (_dragLine == null) return;
        if (_dragLine.LineRenderer == null) return;

        _dragLine.LineRenderer.positionCount = 0;
    }

    private void OnDestroy() => HandleInputEnd();
    private void OnDisable() => HandleInputEnd();
}