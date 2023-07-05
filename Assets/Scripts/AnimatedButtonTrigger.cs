using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatedButtonTrigger : MouseTrigger
{
    [SerializeField]
    private Animator animator;

    protected virtual void Awake()
    {
        activationEvent.AddListener(Push);
        diactivationEvent.AddListener(Pull);
    }
    private void Push()
    {
        if(animator != null)
            animator?.Play("push");
        if (animator != null)
            animator?.SetBool("activated", true);
    }
    private void Pull()
    {
        if (animator != null)
            animator?.SetBool("activated", false);
        if (animator != null)
            animator?.Play("pull");
    }
}
