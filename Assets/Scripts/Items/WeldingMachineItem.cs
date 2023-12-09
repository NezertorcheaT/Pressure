using System;
using Items.Usables;
using UnityEngine;

namespace Items
{
    public class WeldingMachineItem : MonoBehaviour, IUpdateUsableItem
    {
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem ps;
        private Action _onPickUp;
        private Action _onRemove;
        string IItem.ItemName => "Welding Machine";

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

        private void Update()
        {
            animator.SetBool("doing", isUsing);
        }

        bool IUpdateUsableItem.Using
        {
            get => isUsing;
            set => isUsing = value;
        }

        private bool isUsing = false;

        void IUpdateUsableItem.UpdateUse(Action removeThis)
        {
        }

        public void ParticlesPlay()
        {
            ps.Play();
        }
        public void ParticlesStop()
        {
            ps.Stop();
        }
        public void Hit()
        {
            var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
            var hits = Physics.RaycastAll(ray, 5);
            foreach (var hit in hits)
            {
                var trigger = hit.collider.gameObject.GetComponent<Weldable>();

                if (trigger == null) continue;

                trigger.Activate();
                break;
            }
        }
    }
}