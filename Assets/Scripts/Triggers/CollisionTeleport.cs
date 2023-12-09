using UnityEngine;
using Zenject;

public class CollisionTeleport : MonoBehaviour
{
    [SerializeField] private int layer;
    [SerializeField] private Transform pos;
    [Inject] private DiContainer _container;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != layer) return;

        var t = collision.gameObject.GetComponent<BackHachTeleportable>();
        if (t)
        {
            _container.InstantiatePrefab(t.ToTeleportPrefab, pos.position, pos.rotation, null);
            Destroy(collision.gameObject);
        }
        else
        {
            collision.transform.position = pos.position;
            collision.transform.rotation = pos.rotation;
            
            var dr = collision.gameObject.GetComponent<DragRigidbody>();
            if (dr != null)
                dr.HandleInputEnd();
            
            if (!collision.transform.parent) return;
            dr = collision.transform.parent.GetComponent<DragRigidbody>();
            if (dr != null)
                dr.HandleInputEnd();
        }
    }
}