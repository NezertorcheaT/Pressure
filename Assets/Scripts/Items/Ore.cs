using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Ore : MonoBehaviour
{
    public UnityEvent activationEvent;

    public virtual void Activate()
    {
        activationEvent?.Invoke();
        Destroy(gameObject);
    }
}