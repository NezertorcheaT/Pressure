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

    [Inject]
    private void Construct(ItemsShow inv)
    {
        _inventory = inv;
        _inventory.OnSelectionChange += OnSelectionChange;
    }

    private void OnSelectionChange(IItem item)
    {
        if (item != null)
        {
            textObj.text = item.ItemName;
        }
        else
        {
            textObj.text = "";
        }

        if (item as IUIItemPercent != null)
        {
            var prc = (item as IUIItemPercent).percent;

            precentageOff.SetActive(true);
            precentageMax.SetActive(prc != 0);
            
            precentage.transform.localScale =
                new Vector3(prc, precentage.transform.localScale.y, precentage.transform.localScale.z);
        }
        else
        {
            precentageOff.SetActive(false);
        }

        textObj.gameObject.SetActive(_inventory.IsWork);
    }
}