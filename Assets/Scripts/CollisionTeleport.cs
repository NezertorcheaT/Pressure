using UnityEngine;

public class CollisionTeleport : MonoBehaviour
{
    [SerializeField] private int layer;
    [SerializeField] private Transform pos;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != layer) return;

        collision.transform.position = pos.position;
        collision.transform.rotation = pos.rotation;

        var dr = collision.gameObject.GetComponent<DragRigidbody>();
        if (dr != null)
            dr.HandleInputEnd(Input.mousePosition);
    }
}