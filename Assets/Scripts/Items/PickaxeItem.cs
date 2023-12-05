using System;
using Items.Usables;
using UnityEngine;

namespace Items
{
    public class PickaxeItem : MonoBehaviour, IUsableItem
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float mineWeight = 1;
        [SerializeField] private float mineRadius = 1;
        private Action _onPickUp;
        private Action _onRemove;
        string IItem.ItemName => "Pickaxe";

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

        void IUsableItem.Use(Action removeThis)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
                animator.Play("shoot");
        }

        public void Hit()
        {
            Ore trigger;
            var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
            var hits = Physics.RaycastAll(ray, 5);
            foreach (var hit in hits)
            {
                if (hit.collider.transform.parent)
                {
                    var terrain = hit.collider.transform.parent.GetComponent<GenTest>();
                    if (terrain != null)
                    {
                        terrain.Terraform(hit.point, mineWeight, mineRadius);
                    }
                }

                trigger = hit.collider.gameObject.GetComponent<Ore>();

                if (trigger == null) continue;

                trigger.Activate();
                break;
            }
        }
    }
}