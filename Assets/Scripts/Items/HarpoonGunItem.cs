using System;
using UnityEngine;

namespace Items
{
    public class HarpoonGunItem : MonoBehaviour, IUsableItem, IUIItemPercent
    {
        public string ItemName => "Harpoon gun";
        [SerializeField] private int ammo = 10;
        [SerializeField] private int maxAmmo = 10;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform spawnPos;
        [SerializeField] private GameObject harpoonPrefab;
        private bool _canShoot = true;

        public void Use(Action removeThis)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("shoot")) return;
            if (ammo <= 0)
            {
                removeThis?.Invoke();
                return;
            }

            ammo -= 1;
            Instantiate(harpoonPrefab, spawnPos.position, spawnPos.rotation, null);
            animator.Play("shoot");
        }

        public float percent => (float) ammo / (float) maxAmmo;
    }
}