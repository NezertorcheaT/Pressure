using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemsShow : MonoBehaviour
{
    [SerializeField] private int selection = 0;
    [SerializeField] private float lerpSpeed = 1;
    [SerializeField] private Transform itemsPos;
    [SerializeField] private Transform spawnPos;

    [SerializeField, FormerlySerializedAs("Enabled")]
    private bool enbld;

    public bool IsWork => enbld;
    public event Action OnChange;
    public event Action<IItem> OnSelectionChange;
    public List<IItem> Items { get; private set; }

    public void AddItem(IItem item)
    {
        Items.Add(Instantiate(item.gameObject, spawnPos).GetComponent<IItem>());
        selection = Items.Count - 1;
        OnChange?.Invoke();
    }

    public void AddItem(GameObject item)
    {
        Items.Add(Instantiate(item.gameObject, spawnPos).GetComponent<IItem>());
        selection = Items.Count - 1;
        OnChange?.Invoke();
    }

    private void Pop(int i)
    {
        if (Items[i] != null)
            Destroy(Items[i].gameObject);
        Items.RemoveAt(i);
        selection = 0;
        OnChange?.Invoke();
    }

    private void UseUpdateItemOnSlot(int i)
    {
        if (i >= Items.Count) return;
        if (Items[i] == null)
        {
            if (i != 0)
                Items.RemoveAt(i);
            return;
        }

        if ((Items[i] as IUpdateUsableItem) == null)
            return;

        ((IUpdateUsableItem) Items[i]).UpdateUse(() => { Pop(i); });
        OnChange?.Invoke();
    }

    public void UseItemOnSlot(int i)
    {
        if (i >= Items.Count) return;
        if (Items[i] == null)
        {
            if (i != 0)
                Items.RemoveAt(i);
            return;
        }

        if ((Items[i] as IUsableItem) == null)
            return;

        ((IUsableItem) Items[i]).Use(() => { Pop(i); });
        OnChange?.Invoke();
    }

    public void ToggleEnabled()
    {
        enbld.Toggle();
        UpdateSelector();
        SetNormalPos();
    }

    public void OffAll()
    {
        enbld = false;
        UpdateSelector();
        SetNormalPos();
    }

    public void OnAll()
    {
        enbld = true;
        UpdateSelector();
        SetNormalPos();
    }

    private void Start()
    {
        Items = new List<IItem>();
        Items.Add(null);
        OnChange += UpdateSelector;
        OnChange?.Invoke();
        SetNormalPos();
    }

    private void Update()
    {
        if (Items.Count == 1)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            selection += +(int) (Input.GetAxis("Mouse ScrollWheel") * 10);

            if (selection > Items.Count - 1) selection -= Items.Count;
            if (selection < 0) selection = Items.Count + selection;

            UpdateSelector();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
            UseItemOnSlot(selection);
        if (Input.GetKey(KeyCode.Mouse1))
            UseUpdateItemOnSlot(selection);
    }

    private void FixedUpdate()
    {
        if (enbld)
        {
            transform.position = Vector3.Slerp(transform.position, itemsPos.position, Time.fixedDeltaTime * lerpSpeed);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, itemsPos.rotation, Time.fixedDeltaTime * lerpSpeed);
        }
    }

    private void SetNormalPos()
    {
        transform.position = itemsPos.position;
        transform.rotation = itemsPos.rotation;
    }


    private void UpdateSelector()
    {
        OnSelectionChange.Invoke(Items[selection]);
        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i] != null)
                Items[i].gameObject.SetActive(enbld && i == selection);
        }
    }
}