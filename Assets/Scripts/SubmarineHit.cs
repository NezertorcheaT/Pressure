using System;
using UnityEngine;

public class SubmarineHit : MonoBehaviour
{
    public Action<ContactPoint> OnHit;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 0)
            OnHit.Invoke(other.GetContact(0));
    }
}