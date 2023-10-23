using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemsShow : MonoBehaviour
{
    [SerializeField] private Transform[] models;
    [SerializeField] private int selection = 0;
    [SerializeField] private float lerpSpeed = 1;
    [SerializeField] private Transform itemsPos;

    [SerializeField, FormerlySerializedAs("Enabled")]
    private bool enbld = false;

    public event Action OnChange;
    public List<Item> Items { get; private set; }

    public void TryAddItem(Item item)
    {
        Items.Add(item);
    }

    public void UseItemOnSlot(int i)
    {
        if (i >= Items.Count) return;
        if (Items[i] == null)
        {
            Items.RemoveAt(i);
            return;
        }

        Items[i].Use(() => { Items.RemoveAt(i); });
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
        UpdateSelector();
        SetNormalPos();
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            selection += +(int) (Input.GetAxis("Mouse ScrollWheel") * 10);

            if (selection > models.Length - 1) selection -= models.Length;
            if (selection < 0) selection = models.Length + selection;

            UpdateSelector();
        }
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

    private void UpdateInventory()
    {
        throw new NotImplementedException();
    }

    private void UpdateSelector()
    {
        for (int i = 0; i < models.Length; i++)
        {
            models[i].gameObject.SetActive(enbld && i == selection);
        }
    }
}