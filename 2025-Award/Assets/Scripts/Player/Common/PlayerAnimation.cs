using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    // Attackのアニメーションが開始か終了かを判定
    public bool Attack { get; private set; }

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
    /// Attackのアニメーションを呼び出したときに使用するクラス
    /// </summary>
    public void AttackStart()
    {
        Debug.Log("AttackStart");
        Attack = true;
    }

    public void AttackHit()
    {
        _player.Attack.AttackHit();
    }

    /// <summary>
    /// アニメーションが終了したときに呼び出すクラス
    /// Attackのアニメーションにトリガーとして登録しているため呼び出し不要
    /// </summary>
    private void AttackEnd()
    {
        Debug.Log("AttackEnd");
        Attack = false;
    }
}
