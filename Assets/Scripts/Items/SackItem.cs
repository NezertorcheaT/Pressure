using System;
using UnityEngine;
using Zenject;

namespace Items
{
    public class SackItem : MonoBehaviour, IUsableItem, IUIItemPercent
    {
        [SerializeField] private int maxCapacity = 10;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject prefabIn;
        [SerializeField] private Transform spawnPoint;
        private Action _onPickUp;
        private Action _onRemove;
        private int _capacity = 0;
        private FirstPerson _pl;
        string IItem.ItemName => "Sack";

        Action IItem.OnPickUp
        {
            get => _onPickUp;
            set => _onPickUp = value;
        }

        Action IItem.OnRemove
        {
            get => _onRemove;
            set => _onRemove = value;
        }
        [Inject]
        private void Construct(FirstPerson pl)
        {
            _pl = pl;
        }

        void IUsableItem.Use(Action removeThis)
        {
            OreCristal trigger;
            var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
            var hits = Physics.RaycastAll(ray, 5, 5);
            
            foreach (var hit in hits)
            {
                trigger = hit.collider.gameObject.GetComponent<OreCristal>();

                if (trigger == null) continue;

                trigger.Activate();
                _capacity += 1;
                return;
            }
            
            if (_capacity <= 0) return;
            
            Instantiate(_pl.IsUnderWater ? prefab : prefabIn, spawnPoint.position, Quaternion.identity, null);
            _capacity -= 1;
        }

        public float percent => (float) _capacity / (float) maxCapacity;
    }
}