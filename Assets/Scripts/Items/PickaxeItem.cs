using System;
using UnityEngine;

namespace Items
{
    public class PickaxeItem : MonoBehaviour, IUsableItem
    {
        public string ItemName => "Pickaxe";
        [SerializeField] private Animator animator;

        public void Use(Action removeThis)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
                animator.Play("shoot");
        }
    }
}