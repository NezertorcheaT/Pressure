using UnityEngine;
using UnityEngine.Events;

namespace Items.Usables
{
    [RequireComponent(typeof(Collider))]
    public class Weldable : MonoBehaviour
    {
        public UnityEvent activationEvent;

        public virtual void Activate() => activationEvent?.Invoke();

        public void DestroySelf() => Destroy(gameObject);
    }
}