using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textObj;
    [SerializeField] private Image precentage;
    [SerializeField] private GameObject precentageOff;
    [SerializeField] private GameObject precentageMax;
    private ItemsShow _inventory;
    private bool _percen;

    [Inject]
    private void Construct(ItemsShow inv)
    {
        _inventory = inv;
        _inventory.OnSelectionChange += OnSelectionChange;
    }

    private void OnSelectionChange(IItem item)
    {
        textObj.text = item != null ? item.ItemName : "";

        precentageOff.SetActive(_inventory.IsWork && item is IUIItemPercent);
        if (item is IUIItemPercent itemPercent)
        {
            var prc = itemPercent.percent;

            precentageMax.SetActive(prc != 0);

            precentage.transform.localScale =
                new Vector3(prc, precentage.transform.localScale.y, precentage.transform.localScale.z);
        }

        textObj.gameObject.SetActive(_inventory.IsWork);
    }
}