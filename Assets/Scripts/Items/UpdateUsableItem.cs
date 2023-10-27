using System;
using Items;
using UnityEngine;

public class UpdateUsableItem : MonoBehaviour, IUpdateUsableItem, IUIItemPercent
{
    [SerializeField] private string itemName = "ogh";
    [SerializeField] private int maxUses = 1000;
    [SerializeField] private int uses = 0;
    public string ItemName => itemName;

    public void UpdateUse(Action removeThis)
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