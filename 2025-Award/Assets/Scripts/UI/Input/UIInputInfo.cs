using UnityEngine;

public class UIInputInfo
{
    /// <summary>
    /// ポーズする判定
    /// </summary>
    public bool Pause {  get; set; }

    /// <summary>
    /// UIの移動入力の値
    /// </summary>
    public Vector2 Navigate { get; set; }

    /// <summary>
    /// 決定
    /// </summary>
    public bool Submit {  get; set; }

    /// <summary>
    /// 戻る
    /// </summary>
    public bool Cancel { get; set; }
}
