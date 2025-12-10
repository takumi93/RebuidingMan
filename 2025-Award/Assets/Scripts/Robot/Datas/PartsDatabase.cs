using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PartDatabase", menuName = "Scriptable Objects/PartDatabase")]
public class PartsDatabase : ScriptableObject
{
    [SerializeField] private List<PartsData> allParts;

    private Dictionary<int, PartsData> allPartsDict;

    private void OnEnable()
    {
        Init();
    }

    // データの初期化
    public void Init()
    {
        // partName をキーにして Dictionary 化
        allPartsDict = allParts.ToDictionary(p => p.id, p => p);
    }

    // データの検索
    public PartsData GetPartById(int id)
    {
        if (allPartsDict == null)
        {
            Init(); // 未初期化なら初期化
        }

        if (allPartsDict.TryGetValue(id, out var part))
        {
            // 見つかった場合
            return part;
        }
        else
        {
            // 見つからなかった場合
            return null;
        }
    }
}
