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
        animator?.Play("push");
        animator?.SetBool("activated", true);
    }
    private void Pull()
    {
        animator?.SetBool("activated", false);
        animator?.Play("pull");
    }
}
