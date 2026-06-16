using UnityEngine;

public class PlayerInputInfo
{
    /// <summary>
    /// 移動の入力情報
    /// </summary>
    public Vector3 Move {  get; set; }

    /// <summary>
    /// 移動を開始し始めた判定
    /// </summary>
    public bool IsMoving { get; set; }

    /// <summary>
    /// 視点移動の入力情報
    /// </summary>
    public Vector3 Look { get; set; }

    /// <summary>
    /// 攻撃の開始し始めた判定
    /// </summary>
    public bool IsAttack { get; set; }

    /// <summary>
    /// つかむ等の動作の入力情報
    /// </summary>
    public bool Interact { get; set; }

    /// <summary>
    /// 作成の動作の入力情報
    /// </summary>
    public bool Create {  get; set; }

}
