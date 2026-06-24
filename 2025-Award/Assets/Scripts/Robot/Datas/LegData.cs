using UnityEngine;

[CreateAssetMenu(fileName = "LegData", menuName = "Scriptable Objects/LegData")]
public class LegData : PartsData
{
    public LegType legType;

    public override PartsType GetPartsType() => PartsType.Leg;
}

public enum LegType
{
    Normal,         // ノーマル
    Caterpillar     // ホイール
}

