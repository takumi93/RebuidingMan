using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory: MonoBehaviour
{
    // 部品を追加（インベントリ）（既に同じ種類がある場合は無視 or 入れ替え）
    // 今回頭、胴、足ごとに1種類だけのためDictionary型を使用
    private readonly Dictionary<PartsType, PartsData> EquippedParts = new Dictionary<PartsType, PartsData>();

    // 部品データベース
    [SerializeField] private PartsDatabase _partsDatabase;

    [SerializeField] private PlayUI _playUI;

    /// <summary>
    /// 部品を拾った時に呼ぶ処理
    /// </summary>
    /// <param name="part">拾った部品</param>
    /// <returns></returns>
    public bool AddPart(GameObject part)
    {
        // 部品につけてるpartsPickupからIDを取得しデータベースからデータを取得
        var _data = _partsDatabase.GetPartById(part.GetComponent<PartsPickup>().GetPartID());

        // 部品の種類を取得
        var type = _data.GetPartsType();

        // インベントリの部品と同じ時は無視、違う場合はインベントリの部品を落とす
        if (EquippedParts.TryGetValue(type, out var currentPart))
        {
            if (currentPart.Id == _data.Id)
            {
                return false;
            }

            DropPart(type);
        }

        // データを複製する
        var partCopy = ScriptableObject.Instantiate(_data);

        // インベントリに追加
        EquippedParts[_data.GetPartsType()] = partCopy;

        // UI更新通知
        _playUI.RefreshParts(this);

        return true;
    }

    /// <summary>
    /// アイテムを落とす
    /// </summary>
    /// <param name="type"></param>
    private void DropPart(PartsType type)
    {
        // 拾った部品が元々持っている部品と一緒の場合は無視
        if (!EquippedParts.TryGetValue(type, out var oldPart)) return;

        // 元々持っていた部品を落とす
        var droppedPart = Instantiate(oldPart.Prefab, transform.position + transform.forward, Quaternion.identity);

        DropUtility.SetupDroppedPart(droppedPart);

        // インベントリから削除
        EquippedParts.Remove(type);
    }

    /// <summary>
    /// 同じ種類を持っているのか確認
    /// </summary>
    /// <param name="type">種類（頭、胴、足）</param>
    /// <returns>同じだったらTrue、違う場合はfalse</returns>
    public bool HasPartOfType(PartsType type)
    {
        return EquippedParts.ContainsKey(type);
    }

    /// <summary>
    /// 種類を指定して取得する
    /// </summary>
    /// <param name="type">種類（頭、胴、足）</param>
    /// <returns>指定した種類の部品</returns>
    public PartsData GetPartOfType(PartsType type)
    {
        EquippedParts.TryGetValue(type, out var part);
        return part;
    }

    /// <summary>
    /// 部品を落とす
    /// </summary>
    /// <param name="type"></param>
    public void RemovePart(PartsType type)
    {
        EquippedParts.Remove(type);

        // UI更新通知
        _playUI.RefreshParts(this);
    }

    /// <summary>
    /// インベントリのリセット
    /// </summary>
    public void ClearParts()
    {
        EquippedParts.Clear();

        // UI更新通知
        _playUI.RefreshParts(this);
    }

    // 各パーツ専用の参照プロパティ
    public bool HasHead => HasPartOfType(PartsType.Head);
    public bool HasBody => HasPartOfType(PartsType.Body);
    public bool HasLeg => HasPartOfType(PartsType.Leg);
}
