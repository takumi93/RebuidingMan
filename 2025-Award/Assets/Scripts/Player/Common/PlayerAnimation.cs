using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    // Attackのアニメーションが開始か終了かを判定
    public bool Attack { get; private set; }

    [SerializeField] private Player _player;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
        Attack = false;
    }
}
