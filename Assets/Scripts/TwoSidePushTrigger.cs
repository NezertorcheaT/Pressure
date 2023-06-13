using System.Collections;
using UnityEngine;

public class TwoSidePushTrigger : TwoSideMouseTrigger
{
    [SerializeField] private Animator animator;

    protected override void Update()
    {
        base.Update();

        if(animator != null)
        {
            if (CurrentSide == Side.Up) animator.SetInteger("side", 1);
            if (CurrentSide == Side.None) animator.SetInteger("side", 0);
            if (CurrentSide == Side.Down) animator.SetInteger("side", -1);
        }
    }
}
public abstract class TwoSideMouseTrigger : MonoBehaviour
{
    [SerializeField] private MouseTrigger triggerUp;
    [SerializeField] private MouseTrigger triggerDown;
    [SerializeField] private MouseTrigger triggerNone;
    public Side CurrentSide;

    public enum Side { Up, None, Down }

    protected virtual void Update()
    {
        //if (triggerUp.IsActivated) CurrentSide = Side.Up;
        //else if (triggerDown.IsActivated) CurrentSide = Side.Down;
        //else CurrentSide = Side.None;
    }
}