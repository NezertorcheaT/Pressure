using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public abstract class MouseTrigger : MonoBehaviour
{
    public UnityEvent activationEvent;
    public UnityEvent diactivationEvent;
    public UnityEvent stillActivatedEvent;
    [SerializeField] private bool isActivated;
    public bool IsActivated => isActivated;

    public virtual void Activate()
    {
        isActivated = true;
        activationEvent?.Invoke();
    }
    public virtual void Diactivate()
    {
        isActivated = false;
        diactivationEvent?.Invoke();
    }
    private void Update()
    {
        if (isActivated)
            stillActivatedEvent.Invoke();
        Run();
    }
    protected virtual void Run() { }
}