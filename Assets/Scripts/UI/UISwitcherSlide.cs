using UnityEngine;
using UnityEngine.Events;

public class UISwitcherSlide : MonoBehaviour
{
    [SerializeField] private string _name;
    public string ShowName => _name;
    public UnityEvent onSlideEnable;
    public UnityEvent onSlideDisable;
    public UnityEvent onActionDisable;
    public UnityEvent onActionEnable;
}