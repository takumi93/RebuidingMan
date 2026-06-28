using UnityEngine;

public class AttackController : MonoBehaviour
{
    // Attackの開始か終了かを判定
    public bool IsAttacking { get; private set; }

    // 攻撃力
    [SerializeField] private int _attackPower = 50;

    // 攻撃エフェクト
    [SerializeField] private GameObject _hitEffect = null;

    // プレイヤー
    private Player _player;

    public void Init(Player player)
    {
        _player = player;
    }

    public void OnAttackStart()
    {
        IsAttacking = true;
    }

    public void OnAttackEnd()
    {
        IsAttacking = false;
    }

    /// <summary>
    /// AnimationEventから呼び出す関数
    /// </summary>
    public void AttackHit()
    {
        //　攻撃した際に武器は絶対振るので振った音を再生
        _player.Sound.PlaySwing();

        // 識別したオブジェクトが敵以外は無視
        if (!_player.Detection.IsEnemy) return;

        // 視点の先にあるオブジェクトを取得
        RaycastHit hit = _player.Detection.GetHit();

        // オブジェクトにHPがあるか確認
        PartHp partHp = hit.transform.GetComponentInParent<PartHp>();

        if (partHp == null) return;
        
        partHp.ApplyPartDamage(_attackPower, _player.gameObject);

        Vector3 spawnPos = hit.point;

        //ヒットエフェクトを表示。
        if (_hitEffect != null)
        {
            Instantiate(_hitEffect, spawnPos, Quaternion.identity);
        }

        _player.Sound.PlayAttack();
    }
}
