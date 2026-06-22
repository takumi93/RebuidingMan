using UnityEngine;

public class HitEffectUI : UIAnimation
{
    static readonly int HitHash = Animator.StringToHash("Hit");

    /// <summary>
    /// ヒットエフェクトの再生
    /// </summary>
    public void PlayHitEffect()
    {
        SetTrigger(HitHash);
    }
}