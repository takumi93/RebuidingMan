using UnityEngine;

[CreateAssetMenu(fileName = "BodyData", menuName = "Scriptable Objects/BodyData")]
public class BodyData : PartsData
{
    [Header("‘Ì‚Ìİ’è€–Ú")]
    [Tooltip("“ª‚Ìƒ^ƒCƒv")]
    public BodyType bodyType;
    [Tooltip("UŒ‚A‚Ìƒ_ƒ[ƒWŠ„‡")]
    public int damageA;
    [Tooltip("UŒ‚B‚Ìƒ_ƒ[ƒWŠ„‡")]
    public int damageB;
    [Tooltip("UŒ‚A‚ÌƒN[ƒ‹ƒ^ƒCƒ€")]
    public float coolTimeA;
    [Tooltip("UŒ‚B‚ÌƒN[ƒ‹ƒ^ƒCƒ€")]
    public float coolTimeB;
    [Tooltip("UŒ‚A‚ÌUŒ‚€”õŠÔ")]
    public float preparationTimeA;
    [Tooltip("UŒ‚B‚ÌUŒ‚€”õŠÔ")]
    public float preparationTimeB;
    [Tooltip("UŒ‚A‚ÌUŒ‚”ÍˆÍ”­¶ŠÔ")]
    public float occurrenceTimeA;
    [Tooltip("UŒ‚B‚ÌUŒ‚”ÍˆÍ”­¶ŠÔ")]
    public float occurrenceTimeB;
    [Tooltip("UŒ‚A‚ÌUŒ‚I—¹ŠÔ")]
    public float finishTimeA;
    [Tooltip("UŒ‚B‚ÌUŒ‚I—¹ŠÔ")]
    public float finishTimeB;
    [Tooltip("UŒ‚A‚ÌUŒ‚ƒTƒEƒ“ƒh")]
    public AudioClip attackSoundA;
    [Tooltip("UŒ‚B‚ÌUŒ‚ƒTƒEƒ“ƒh")]
    public AudioClip attackSoundB;
    [Tooltip("UŒ‚‚É“ü‚é‹——£")]
    public int AttackRange;

    public override PartsType GetPartsType() => PartsType.Body;
}

public enum BodyType
{
    Normal, // ƒm[ƒ}ƒ‹
    Gun,    // e
    Axe     // •€
}
