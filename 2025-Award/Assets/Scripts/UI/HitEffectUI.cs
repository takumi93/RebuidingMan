using UnityEngine;

public class HitEffectUI : UIAnimation
{
    static readonly int HitHash = Animator.StringToHash("Hit");

    public override void Init()
    {
        base.Init();
    }

    /// <summary>
    /// ヒットエフェクトの再生
    /// </summary>
    public void PlayHitEffect()
    {
        SetTrigger(HitHash);
    }
}