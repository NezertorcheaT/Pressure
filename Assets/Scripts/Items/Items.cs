using System;
using UnityEngine;

namespace Items
{
    public interface IUpdateUsableItem : IItem
    {
        public void UpdateUse(Action removeThis, FirstPerson pl);
    }

    public interface IUsableItem : IItem
    {
        public void Use(Action removeThis, FirstPerson pl);
    }

    public interface IItem
    {
        public GameObject gameObject { get; }
        public string ItemName { get; }
        public Action OnPickUp { get; set; }
        public Action OnRemove { get; set; }
    }

    public interface IUIItemPercent : IItem
    {
        public float percent { get; }
    }
}