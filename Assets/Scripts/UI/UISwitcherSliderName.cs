using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UISwitcherSliderName : MonoBehaviour
{
    [FormerlySerializedAs("swither")] [SerializeField]
    private UISwitcher switcher;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string left;
    [SerializeField] private string right;

    private void UpdateText(int idx, GameObject current)
    {
        if (current.GetComponent<UISwitcherSlide>())
        {
            text.text = left + current.GetComponent<UISwitcherSlide>().ShowName + right;
        }
        text.text = left + current.name + right;
    }

    private void Subscribe() => switcher.OnSlideChanged += UpdateText;
    private void UnSubscribe() => switcher.OnSlideChanged -= UpdateText;

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