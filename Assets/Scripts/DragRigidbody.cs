using System.ComponentModel;
using UnityEngine;
using Zenject;

// Script created by https://youtube.com/c/Boxply

[RequireComponent(typeof(Rigidbody))]
public class DragRigidbody : MonoBehaviour
{
    [SerializeField] private float force = 600;
    [SerializeField] private float damping = 6;
    [SerializeField] private float distance = 15;

    private DragLine dragLine;
    private ConfigurableJoint joint;
    private float dragDepth;
    private Transform hitTransform;

    [Inject]
    private void Construct(DragLine dragLine)
    {
        this.dragLine = dragLine;
    }

    private void OnMouseDown()
    {
        HandleInputBegin(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        HandleInputEnd(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        HandleInput(Input.mousePosition);
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
                hitTransform = go.transform;

                dragDepth = CameraPlane.CameraToPointDepth(Camera.main, hit.point);
                joint = AttachJoint(hit.rigidbody, hit.point);
            }
        }

        dragLine.lr.positionCount = 2;
    }

    public void HandleInput(Vector3 screenPosition)
    {
        if (joint == null)
            return;
        var worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        joint.transform.position = CameraPlane.ScreenToWorldPlanePoint(Camera.main, dragDepth, screenPosition);

        DrawRope();
    }

    public void HandleInputEnd(Vector3 screenPosition)
    {
        DestroyRope();
        if (hitTransform)
        {
            Destroy(hitTransform.gameObject);
            hitTransform = null;
        }
        if (joint)
            Destroy(joint.gameObject);
    }

    ConfigurableJoint AttachJoint(Rigidbody rb, Vector3 attachmentPosition)
    {
        GameObject go = new GameObject("Attachment Point");
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

    private JointDrive NewJointDrive(float force, float damping)
    {
        JointDrive drive = new JointDrive();
        drive.mode = JointDriveMode.Position;
        drive.positionSpring = force;
        drive.positionDamper = damping;
        drive.maximumForce = Mathf.Infinity;
        return drive;
    }

    private void DrawRope()
    {
        if (joint == null)
        {
            return;
        }

        dragLine.lr.SetPosition(0, dragLine.lineRenderLocation.position);
        dragLine.lr.SetPosition(1, hitTransform.position);
    }

    private void DestroyRope()
    {
        dragLine.lr.positionCount = 0;
    }
}
