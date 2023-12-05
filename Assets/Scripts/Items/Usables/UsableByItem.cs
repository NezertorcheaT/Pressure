using UnityEngine;
using UnityEngine.Events;

namespace Items.Usables
{
    [RequireComponent(typeof(Collider))]
    public class UsableByItem : MonoBehaviour
    {
        public UnityEvent activationEvent;

        public virtual void Activate()
        {
            activationEvent?.Invoke();
        }
    }
}
