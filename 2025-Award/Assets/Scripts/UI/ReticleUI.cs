using UnityEngine;
using UnityEngine.UI;

public class ReticleUI : UIAnimation
{
    [SerializeField] private Image _reticle;

    static readonly int HitHash = Animator.StringToHash("Hit");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// ヒットマーカーの表示するアニメーションを再生する
    /// </summary>
    public void PlayHitMarker()
    {
        SetTrigger(HitHash);
    }
    
    /// <summary>
    /// レティクルの色を変える
    /// </summary>
    /// <param name="color"></param>
    public void SetReticleColor(Color color)
    {
        _reticle.color = color;
    }
}
