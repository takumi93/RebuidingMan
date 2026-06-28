using UnityEngine;

public abstract class PartBase : MonoBehaviour
{
    // 部品のID
    [SerializeField] protected int _id;

    // オブジェクトに武器のRenderもあるため変更するRenderを指定する
    [Header("変更するマテリアル")]
    [SerializeField] protected Renderer[] _renderers;

    // 参照用
    public int Id => _id;

    /// <summary>
    /// プレイヤー陣営になった時マテリアルを変更する
    /// </summary>
    /// <param name="partsData">変更するパーツデータ</param>
    protected void UpdateMaterial(PartsData partsData)
    {
        var renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in _renderers)
        {
            renderer.sharedMaterial = partsData.AllyMaterial;
        }
    }
}