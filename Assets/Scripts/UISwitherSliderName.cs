using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISwitherSliderName : MonoBehaviour
{
    [SerializeField] private UISwither swither;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string left;
    [SerializeField] private string right;

    private void UpdateText(int idx, string name)
    {
        text.text = left + name + right;
    }

    private void Subscribe() => swither.onSlideChanged += UpdateText;
    private void UnSubscribe() => swither.onSlideChanged -= UpdateText;

    private void OnDestroy()
    {
        UnSubscribe();
    }
    private void OnDisable()
    {
        UnSubscribe();
    }
    private void OnEnable()
    {
        Subscribe();
    }
    private void Start()
    {
        Subscribe();
    }
}
