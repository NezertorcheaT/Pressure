using System;
using UnityEngine;

public class SubmarineHit : MonoBehaviour
{
    public Action<Vector3> OnHit;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 0)
            OnHit.Invoke(transform.InverseTransformPoint(other.GetContact(0).point));
    }
}