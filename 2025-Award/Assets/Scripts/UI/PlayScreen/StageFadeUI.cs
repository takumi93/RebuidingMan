using UnityEngine;

public class StageFadeUI : UIAnimation
{
    private static readonly int FadeInHash = Animator.StringToHash("FadeIn");
    private static readonly int FadeOutHash = Animator.StringToHash("FadeOut");

    public void FadeIn()
    {
        SetTrigger(FadeInHash);
    }

    public void FadeOut()
    {
        SetTrigger(FadeOutHash);
    }
}