using System;
using Items;
using UnityEngine;

public class Item : MonoBehaviour, IUsableItem
{
    [SerializeField] private string itemName = "ogh";
    public string ItemName => itemName;

    public void Use(Action removeThis)
    {
        UsableByItem trigger;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray, 5, 5);
        foreach (var hit in hits)
        {
            trigger = hit.collider.gameObject.GetComponent<UsableByItem>();

            if (trigger == null) continue;

            trigger.Activate();
            Debug.Log(itemName + " Used");
            break;
        }

        //removeThis?.Invoke();
    }
}