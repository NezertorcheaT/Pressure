using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISwither : MonoBehaviour
{
    [SerializeField] private MouseTrigger nextButton;
    [SerializeField] private MouseTrigger prevButton;
    [SerializeField] private Transform forSlides;

    private void Start()
    {
        ActivateOnIndx(0);

        nextButton.activationEvent.AddListener(Next);
        prevButton.activationEvent.AddListener(Prev);
    }

    private int GetIndx()
    {
        int n = 0;
        for (int i = 0; i < forSlides.childCount; i++)
        {
            if (forSlides.GetChild(i).gameObject.activeSelf)
                return i;
        }
        return n;
    }
    private void ActivateOnIndx(int indx)
    {
        if (forSlides.childCount == 0) return;

        for (int i = 0; i < forSlides.childCount; i++)
        {
            forSlides.GetChild(i).gameObject.SetActive(false);
        }
        forSlides.GetChild((int)Mathf.Repeat(indx, forSlides.childCount)).gameObject.SetActive(true);
    }

    private void Next()
    {
        if (forSlides.childCount == 0) return;
        if (forSlides.childCount == 1) ActivateOnIndx(0);
        else ActivateOnIndx(GetIndx() + 1);
    }
    private void Prev()
    {
        if (forSlides.childCount == 0) return;
        if (forSlides.childCount == 1) ActivateOnIndx(0);
        else ActivateOnIndx(GetIndx() - 1);
    }
}
