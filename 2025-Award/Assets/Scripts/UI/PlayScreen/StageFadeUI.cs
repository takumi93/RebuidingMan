using UnityEngine;
using UnityEngine.Events;

public class StageFadeUI : UIAnimation
{
    // ステージが開始されたときに発生する UnityEvent
    public UnityEvent OnFadeInFinished;

    private static readonly int FadeOutHash = Animator.StringToHash("FadeOut");

    public void FadeOut()
    {
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        SetTrigger(FadeOutHash);
    }

    // StageIntro アニメーション内のイベントから呼び出されます。
    public void OnFadeInAnimationEnd()
    {
        OnFadeInFinished?.Invoke();
    }
}