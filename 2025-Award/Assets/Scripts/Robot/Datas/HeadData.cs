using UnityEngine;

public enum HeadType
{
    Pawn,   // “ª
    Rook,   // ‘Ì
    Knight  // ‘«
}

[CreateAssetMenu(fileName = "HeadData", menuName = "Scriptable Objects/HeadData")]
public class HeadData : PartsData
{
    [Tooltip("õ“G”ÍˆÍ")]
    public float radius;
    [Tooltip("õ“G”ÍˆÍ")]
    public float distance;
    [Tooltip("“ª‚Ìƒ^ƒCƒv")]
    public HeadType headType;

    public override PartsType GetPartsType() => PartsType.Head;
}
