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
            Debug.Log(pos.position);
            Debug.Log(collision.transform.position);
            collision.transform.position = pos.position;
            Debug.Log(collision.transform.position);
            collision.transform.rotation = pos.rotation;
        }
    }
}
