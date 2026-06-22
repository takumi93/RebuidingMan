using UnityEngine;

public abstract class UIAnimation : MonoBehaviour
{
    protected Animator _animator;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// アニメーションの再生
    /// </summary>
    /// <param name="trigger"></param>
    protected void SetTrigger(int trigger)
    {
        _animator.SetTrigger(trigger);
    }

    /// <summary>
    /// アニメーションの再生
    /// </summary>
    /// <param name="trigger"></param>
    protected void Play(string stateName)
    {
        _animator.Play(stateName, 0, 0);
    }
}