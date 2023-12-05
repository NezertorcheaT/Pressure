using System;
using Items;
using UnityEngine;

public class UpdateUsableItem : MonoBehaviour, IUpdateUsableItem, IUIItemPercent
{
    [SerializeField] private string itemName = "ogh";
    [SerializeField] private int maxUses = 1000;
    [SerializeField] private int uses = 0;
    private Action _onPickUp;
    private Action _onRemove;
    public string ItemName => itemName;

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

    bool IUpdateUsableItem.Using { get; set; }

    void IUpdateUsableItem.UpdateUse(Action removeThis)
    {
        uses++;
        percent = (float) uses / (float) maxUses;
        if (uses >= maxUses)
        {
            removeThis?.Invoke();
        }
    }

    public float percent { get; private set; }
}