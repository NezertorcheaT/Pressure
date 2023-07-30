using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider))]
public abstract class MouseTrigger : MonoBehaviour
{
    public UnityEvent activationEvent;
    public UnityEvent diactivationEvent;
    public UnityEvent stillActivatedEvent;
    [SerializeField] private bool isActivated;
    [FormerlySerializedAs("TodoText")] [SerializeField] private string todoText;
    public string TodoString => todoText;

    public bool IsActivated => isActivated;

    public virtual void Activate()
    {
        activationEvent?.Invoke();
        isActivated = true;
    }
    public virtual void Diactivate()
    {
        diactivationEvent?.Invoke();
        isActivated = false;
    }
    private void Update()
    {
        if (isActivated)
            stillActivatedEvent.Invoke();
        Run();
    }
    protected virtual void Run() { }
}