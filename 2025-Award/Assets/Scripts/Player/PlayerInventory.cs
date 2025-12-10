using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInventory: MonoBehaviour
{
    // パーツを追加（既に同じ種類がある場合は無視 or 入れ替え）
    public List<PartsData> equippedParts = new List<PartsData>();

    [SerializeField]private PartsDatabase _partsDatabase;

    private PartsData _data { get; set; }

    public bool AddPart(GameObject part)
    {
        _data = _partsDatabase.GetPartById(part.GetComponent<PartsPickup>().GetPartID());

        if (HasPartOfType(_data.GetPartsType()))
        {
            return false;
        }
        var partCopy = ScriptableObject.Instantiate(_data);
        equippedParts.Add(partCopy);

        return true;
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
