using System;
using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    [SerializeField] private MouseTrigger nextButton;
    [SerializeField] private MouseTrigger prevButton;
    [SerializeField] private Transform forSlides;
    public event Action<int, GameObject> OnSlideChanged;

    private void Start()
    {
        ActivateOnIndex(0);

        nextButton.activationEvent.AddListener(Next);
        prevButton.activationEvent.AddListener(Prev);
    }

    private int GetIndex()
    {
        var n = 0;
        for (var i = 0; i < forSlides.childCount; i++)
        {
            if (forSlides.GetChild(i).gameObject.activeSelf)
                return i;
        }

        return n;
    }

    private void ActivateOnIndex(int index)
    {
        if (forSlides.childCount == 0) return;

        for (var i = 0; i < forSlides.childCount; i++)
        {
            forSlides.GetChild(i).gameObject.SetActive(false);
            forSlides.GetChild(i).GetComponent<UISwitcherSlide>()?.onSlideDisable.Invoke();
        }

        var childIndex = (int) Mathf.Repeat(index, forSlides.childCount);
        var currentSlide = forSlides.GetChild(childIndex).gameObject;

        currentSlide.SetActive(true);
        currentSlide.GetComponent<UISwitcherSlide>()?.onSlideEnable.Invoke();

        OnSlideChanged?.Invoke(childIndex, currentSlide);
    }

    private void Next()
    {
        if (forSlides.childCount == 0) return;
        if (forSlides.childCount == 1) ActivateOnIndex(0);
        else ActivateOnIndex(GetIndex() + 1);
    }

    private void Prev()
    {
        if (forSlides.childCount == 0) return;
        if (forSlides.childCount == 1) ActivateOnIndex(0);
        else ActivateOnIndex(GetIndex() - 1);
    }
}