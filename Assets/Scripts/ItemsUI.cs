using TMPro;
using UnityEngine;
using Zenject;

public class ItemsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textObj;
    private ItemsShow _inventory;

    [Inject]
    private void Construct(ItemsShow inv)
    {
        _inventory = inv;
        _inventory.OnSelectionChange += OnSelectionChange;
    }

    private void OnSelectionChange(int i)
    {
        if (_inventory.Items[i] != null)
        {
            textObj.text = _inventory.Items[i].ItemName;
        }
        else
        {
            textObj.text = "";
        }

        textObj.gameObject.SetActive(_inventory.IsWork);
    }
}