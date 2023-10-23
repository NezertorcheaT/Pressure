using System;
using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    [SerializeField] private string itemName = "ogh";
    public string ItemName => itemName;

    public virtual void Use(Action removeThis)
    {
        Debug.Log(itemName+" Used");
        removeThis?.Invoke();
    }
}

public interface IItem
{
    public void Use(Action removeThis);
    public GameObject gameObject { get; }
    public string ItemName { get; }
}