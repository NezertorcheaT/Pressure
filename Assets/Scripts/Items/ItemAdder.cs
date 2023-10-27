using UnityEngine;
using Zenject;

public class ItemAdder : MouseTrigger
{
    [SerializeField] private GameObject itemPrefab;
    private ItemsShow _inventory;

    [Inject]
    private void Construct(ItemsShow inv)
    {
        _inventory = inv;
    }

    private void Start()
    {
        activationEvent.AddListener(Add);
    }

    private void Add()
    {
        _inventory.AddItem(itemPrefab);
        Destroy(gameObject);
    }
}