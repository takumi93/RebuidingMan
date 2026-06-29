using UnityEngine;

public class RobotAnimation : MonoBehaviour
{
    // アニメーション
    private Animator _animator;
    // 武器
    private WeaponBase _weapon;
    // 胴体
    private BodyBase _body;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 初期化（胴体だけ使用）
    /// </summary>
    /// <param name="body"></param>
    public void InitBody(BodyBase body)
    {
        Init();

        _weapon = GetComponentInChildren<WeaponBase>();
        _body = body;
    }

    /// <summary>
    /// アニメーションを再生する
    /// </summary>
    /// <param name="name">再生するトリガー名</param>
    public void SetTrigger(string name)
    {
        _animator.SetTrigger(name);
    }

    /// <summary>
    /// アニメーションを再生する
    /// 移動時連打すると攻撃アニメーションが再生されないためBoolで設定
    /// </summary>
    /// <param name="name">AnimationのParameter</param>
    /// <param name="onMove">ParameterのOnOff</param>
    public void SetBool(string name, bool onMove)
    {
        _animator.SetBool(name, onMove);
    }

    /// <summary>
    /// アニメーションの無効化
    /// </summary>
    public void DestoryAnimation()
    {
        _animator.enabled = false;
    }

    /// <summary>
    /// AnimationEventで使用する攻撃の開始イベント
    /// </summary>
    public void OnAttackStart()
    {
        _body?.OnAttackStart();
    }

    /// <summary>
    /// AnimationEventで使用する攻撃の当たり判定表示イベント
    /// </summary>
    public void HitOn()
    {
        _weapon?.HitOn();
    }

    /// <summary>
    /// AnimationEventで使用する攻撃の当たり判定終了イベント
    /// </summary>
    public void HitOff()
    {
        _weapon?.HitOff();
    }

    /// <summary>
    /// AnimationEventで使用する弾を発射するイベント
    /// </summary>
    public void AttackAEvent()
    {
        _body?.AttackAEvent();
    }

    /// <summary>
    /// AnimationEventで使用する弾を発射するイベント
    /// </summary>
    public void AttackBEvent()
    {
        _body?.AttackBEvent();
    }

    /// <summary>
    /// AnimationEventで使用する攻撃の終了イベント
    /// </summary>
    public void OnAttackEnd()
    {
        _body?.OnAttackEnd();
    }
}
