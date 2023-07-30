using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatedButtonTrigger : MouseTrigger
{
    [SerializeField] private Animator animator;
    private static readonly int Activated = Animator.StringToHash("activated");

    protected virtual void Awake()
    {
        activationEvent.AddListener(Push);
        diactivationEvent.AddListener(Pull);
    }

    private void Push()
    {
        if (animator != null)
            animator.Play("push");
        if (animator != null)
            animator.SetBool(Activated, true);
    }

    private void Pull()
    {
        if (animator != null)
            animator.SetBool(Activated, false);
        if (animator != null)
            animator.Play("pull");
    }
}