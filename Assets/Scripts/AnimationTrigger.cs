using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AnimationTrigger : MouseTrigger
{
    [FormerlySerializedAs("_animator")] [SerializeField] private Animator animator;
    [FormerlySerializedAs("_animation")] [SerializeField] private new string animation;
    [SerializeField] private bool isAnimating = false;
    private bool _isAnimatingStart = false;
    public UnityEvent onAnimationStart;
    public UnityEvent onAnimationEnd;

    private void Start()
    {
        //_animator.enabled = false;
        activationEvent.AddListener(Move);
    }
    private void Update()
    {
        if (!_isAnimatingStart && ((isAnimating && animator.GetCurrentAnimatorStateInfo(0).IsName(animation) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) || (isAnimating && !animator.GetCurrentAnimatorStateInfo(0).IsName(animation))))
        {
            isAnimating = false;
            //_animator.enabled = false;
            onAnimationEnd.Invoke();
        }
    }
    private void FixedUpdate()
    {
        _isAnimatingStart = false;
    }
    private void Move()
    {
        _isAnimatingStart = true;
        //_animator.enabled = true;
        animator.Play(animation);
        isAnimating = true;
        onAnimationStart.Invoke();
    }
}
