using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTrigger : MouseTrigger
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animation;
    [SerializeField] private bool isAnimating = false;
    private bool isAnimatingStart = false;
    public UnityEvent onAnimationStart;
    public UnityEvent onAnimationEnd;

    private void Start()
    {
        //_animator.enabled = false;
        activationEvent.AddListener(Move);
    }
    private void Update()
    {
        if (!isAnimatingStart && ((isAnimating && _animator.GetCurrentAnimatorStateInfo(0).IsName(_animation) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) || (isAnimating && !_animator.GetCurrentAnimatorStateInfo(0).IsName(_animation))))
        {
            isAnimating = false;
            //_animator.enabled = false;
            onAnimationEnd.Invoke();
        }
    }
    private void FixedUpdate()
    {
        isAnimatingStart = false;
    }
    private void Move()
    {
        isAnimatingStart = true;
        //_animator.enabled = true;
        _animator.Play(_animation);
        isAnimating = true;
        onAnimationStart.Invoke();
    }
}
