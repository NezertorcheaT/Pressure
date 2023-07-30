using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TorpedoChecker : MonoBehaviour
{
    [SerializeField] private bool hasTorpedo;
    private GameObject _torpedo;
    public bool HasTorpedo => hasTorpedo;
    public GameObject CurrentTorpedo => _torpedo;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Torpedo"))
        {
            _torpedo = other.transform.parent != null ? other.transform.parent.gameObject : other.gameObject;
            hasTorpedo = true;
        }
        else
        {
            _torpedo = null;
            hasTorpedo = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Torpedo")) return;

        _torpedo = other.transform.parent != null ? other.transform.parent.gameObject : other.gameObject;
        hasTorpedo = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Torpedo")) return;

        _torpedo = null;
        hasTorpedo = false;
    }

    public void DestroyCurrentTorpedo()
    {
        if (!hasTorpedo || !_torpedo) return;

        hasTorpedo = false;
        Destroy(_torpedo);
        _torpedo = null;
    }
}