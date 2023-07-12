using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TorpedoChecker : MonoBehaviour
{
    [SerializeField] private bool hasTorpedo = false;
    private GameObject torpedo = null;
    public bool HasTorpedo => hasTorpedo;
    public GameObject CurrentTorpedo => torpedo;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Torpedo"))
        {
            torpedo = other.gameObject;
            hasTorpedo = true;
        }
        else
        {
            torpedo = null;
            hasTorpedo = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Torpedo"))
        {
            torpedo = other.gameObject;
            hasTorpedo = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Torpedo"))
        {
            torpedo = null;
            hasTorpedo = false;
        }
    }
    public void DestroyCurrentTorpedo()
    {
        if (hasTorpedo && torpedo)
            Destroy(torpedo);
    }
}
