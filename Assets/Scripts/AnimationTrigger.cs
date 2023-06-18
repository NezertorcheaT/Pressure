using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTrigger : MouseTrigger
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animation;
    [SerializeField] private bool isAnimating = false;
    public UnityEvent onAnimationStart;
    public UnityEvent onAnimationEnd;

    private void Start()
    {
        _animator.enabled = false;
        activationEvent.AddListener(Move);
    }
    private void Update()
    {
        if (isAnimating && _animator.GetCurrentAnimatorStateInfo(0).IsName(_animation) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            isAnimating = false;
            _animator.enabled = false;
            onAnimationEnd.Invoke();
        }
    }
    private void Move()
    {
        _animator.enabled = true;
        _animator.Play(_animation);
        isAnimating = true;
        onAnimationStart.Invoke();
    }
}
