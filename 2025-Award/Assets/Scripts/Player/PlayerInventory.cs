using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

public class PlayerInventory: MonoBehaviour
{
    // パーツを追加（既に同じ種類がある場合は無視 or 入れ替え）
    public List<PartsData> equippedParts = new List<PartsData>();

    public void AddPart(PartsData part)
    {
        if (HasPartOfType(part.GetPartsType()))
        {
            return;
        }
        var partCopy = ScriptableObject.Instantiate(part);
        equippedParts.Add(partCopy);
    }

    public bool HasPartOfType(PartsType type)
    {
        return equippedParts.Any(p => p.GetPartsType() == type);
    }

    public PartsData GetPartOfType(PartsType type)
    {
        return equippedParts.FirstOrDefault(p => p.GetPartsType() == type);
    }

    public void ClearParts()
    {
        equippedParts.Clear();
    }

    // 各パーツ専用の参照プロパティ
    public bool HasHead => HasPartOfType(PartsType.Head);
    public bool HasBody => HasPartOfType(PartsType.Body);
    public bool HasLeg => HasPartOfType(PartsType.Leg);
}
