using System;
using UnityEngine;
using Zenject;

namespace Items
{
    public class HarpoonGunItem : MonoBehaviour, IUsableItem, IUIItemPercent
    {
        [Inject]
        private void Construct(FirstPerson pl,DiContainer container)
        {
            _pl = pl;
            _container = container;
        }

        string IItem.ItemName => "Harpoon gun";

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

        [SerializeField] private int ammo = 10;
        [SerializeField] private int maxAmmo = 10;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform spawnPos;
        [SerializeField] private GameObject harpoonPrefab;
        private bool _canShoot = true;
        private Action _onPickUp;
        private Action _onRemove;
        private FirstPerson _pl;
        private DiContainer _container;
        
        void IUsableItem.Use(Action removeThis)
        {
            if (!_pl.IsUnderWater) return;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("shoot")) return;
            if (ammo <= 0)
            {
                removeThis?.Invoke();
                return;
            }

            ammo -= 1;
            _container.InstantiatePrefab(harpoonPrefab, spawnPos.position, spawnPos.rotation, null);
            animator.Play("shoot");
        }

        public float percent => (float) ammo / (float) maxAmmo;
    }
}