using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public void Use(Action RemoveItem)
    {
        RemoveItem?.Invoke();
    }
}