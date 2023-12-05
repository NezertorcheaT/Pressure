using UnityEngine;
using UnityEngine.Events;

namespace Items.Usables
{
    [RequireComponent(typeof(Collider))]
    public class Ore : MonoBehaviour
    {
        public UnityEvent activationEvent;
        [SerializeField] private GameObject orePrefab;
        [SerializeField] private int maxOre = 10;

        public virtual void Activate()
        {
            activationEvent?.Invoke();
            var m = Random.Range(maxOre / 2, maxOre);
            for (var i = 0; i < m; i++)
            {
                Instantiate(orePrefab, transform.position, Quaternion.identity, null);
            }

            Destroy(gameObject);
        }
    }
}