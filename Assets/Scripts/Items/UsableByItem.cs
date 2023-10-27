using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class UsableByItem : MonoBehaviour
{
    public UnityEvent activationEvent;

    public virtual void Activate()
    {
        activationEvent?.Invoke();
    }
}
