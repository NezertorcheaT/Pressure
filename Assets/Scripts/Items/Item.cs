using System;
using Items;
using UnityEngine;

public class Item : MonoBehaviour, IUsableItem
{
    [SerializeField] private string itemName = "ogh";
    private Action _onPickUp;
    private Action _onRemove;
    string IItem.ItemName => itemName;

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
        UsableByItem trigger;
        var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
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