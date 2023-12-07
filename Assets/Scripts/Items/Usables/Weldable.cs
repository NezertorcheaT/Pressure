using UnityEngine;
using UnityEngine.Events;

namespace Items.Usables
{
    [RequireComponent(typeof(Collider))]
    public class Weldable : MonoBehaviour
    {
        public Weldable otherSide;
        public UnityEvent activationEvent;

        public void Activate()
        {
            otherSide?.activationEvent?.Invoke();
            activationEvent?.Invoke();
        }

        public void DestroySelf()
        {
            if (otherSide)
                Destroy(otherSide.gameObject);
            Destroy(gameObject);
        }
    }
}