using UnityEngine;

public class CollisionTeleporter : MonoBehaviour
{
    [SerializeField] private int layer;
    [SerializeField] private Transform pos;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.layer);
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.layer == layer)
        {
            collision.transform.position = pos.position;
            collision.transform.rotation = pos.rotation;

            var dr = collision.gameObject.GetComponent<DragRigidbody>();
            if (dr != null)
                dr.HandleInputEnd(Input.mousePosition);
        }
    }
}
