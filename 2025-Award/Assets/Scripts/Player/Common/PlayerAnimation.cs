using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    // Attackのアニメーションが開始か終了かを判定
    public bool IsAttacking { get; private set; }

    private Player _player;

    public void Init(Player player)
    {
        _animator = GetComponent<Animator>();
        _player = player;
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
    /// AnimationEventで使用する攻撃の開始イベント
    /// </summary>
    private void OnAttackStart()
    {
        Debug.Log("AttackStart");
        _player.Attack.OnAttackStart();
    }

    private void AttackHit()
    {
        _player.Attack.AttackHit();
    }

    /// <summary>
    /// AnimationEventで使用する攻撃の終了イベント
    /// </summary>
    private void OnAttackEnd()
    {
        Debug.Log("AttackEnd");
        _player.Attack.OnAttackEnd();
    }
}
