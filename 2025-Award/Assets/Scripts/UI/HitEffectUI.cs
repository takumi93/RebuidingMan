using UnityEngine;

public class HitEffectUI : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    static readonly int HitHash = Animator.StringToHash("Hit");

    public void Play()
    {
        _animator.SetTrigger(HitHash);
    }
}