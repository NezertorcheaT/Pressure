using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Items.Usables
{
    [RequireComponent(typeof(Collider))]
    public class Ore : MonoBehaviour
    {
        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }

        public UnityEvent activationEvent;
        [SerializeField] private GameObject orePrefab;
        [SerializeField] private GameObject particlePrefab;
        [SerializeField] private int maxOre = 10;
        private DiContainer _container;

        public virtual void Activate()
        {
            activationEvent?.Invoke();
            var m = Random.Range(maxOre / 2, maxOre);
            _container.InstantiatePrefab(particlePrefab, transform.position, Quaternion.identity, null);
            for (var i = 0; i < m; i++)
            {
                _container.InstantiatePrefab(orePrefab, transform.position, Quaternion.identity, null);
            }

            Destroy(gameObject);
        }
    }
}