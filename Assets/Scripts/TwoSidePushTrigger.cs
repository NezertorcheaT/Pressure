using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TwoSidePushTrigger : TwoSideMouseTrigger
{
    [SerializeField] private Animator animator;

    protected override void Update()
    {
        base.Update();

        if (animator != null)
        {
            if (currentSide == Side.Up) animator.SetInteger("side", 1);
            if (currentSide == Side.None) animator.SetInteger("side", 0);
            if (currentSide == Side.Down) animator.SetInteger("side", -1);
        }
    }
}

public abstract class TwoSideMouseTrigger : MonoBehaviour
{
    public enum Side
    {
        Up,
        None,
        Down
    }

    [SerializeField] private MouseTrigger triggerUp;
    [SerializeField] private MouseTrigger triggerDown;
    [SerializeField] private MouseTrigger triggerNone;
    [FormerlySerializedAs("CurrentSide")] public Side currentSide;

    public UnityEvent onSideUp;
    public UnityEvent onSideDown;
    public UnityEvent onSideNone;

    private void Start()
    {
        triggerUp.diactivationEvent.AddListener(OnUp);
        triggerDown.diactivationEvent.AddListener(OnDown);
        triggerNone.diactivationEvent.AddListener(OnNone);
    }

    protected virtual void Update()
    {
        //if (triggerUp.IsActivated) CurrentSide = Side.Up;
        //else if (triggerDown.IsActivated) CurrentSide = Side.Down;
        //else CurrentSide = Side.None;
    }

    private void OnUp()
    {
        //if (CurrentSide!=Side.Up)
        onSideUp.Invoke();
    }

    private void OnDown()
    {
        //if (CurrentSide != Side.Down)
        onSideDown.Invoke();
    }

    private void OnNone()
    {
        //if (CurrentSide != Side.None)
        onSideNone.Invoke();
    }
}