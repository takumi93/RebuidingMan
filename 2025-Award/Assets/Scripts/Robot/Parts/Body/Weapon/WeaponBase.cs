using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected Robot _owner;
    protected BodyBase _body;
    protected TeamObject _teamObject;

    // 武器に当たった判定を残すための変数
    private HashSet<GameObject> _hitTargets = new HashSet<GameObject>();

    /// <summary>
    /// ダメージ量
    /// </summary>
    protected virtual int AttackDamage => _body.Damage;

    /// <summary>
    /// 武器の所有者（攻撃してきた敵の情報を渡すため）
    /// </summary>
    protected virtual GameObject Attacker => _owner.gameObject;

    /// <summary>
    /// 初期化
    /// 近接武器用
    /// </summary>
    public virtual void Init()
    {
        _owner = GetComponentInParent<Robot>();
        _body = _owner.Body;
        _teamObject = _owner.GetComponent<TeamObject>();
    }

    /// <summary>
    /// 初期化
    /// 飛び道具用
    /// </summary>
    public virtual void Init(Robot owner)
    {
        _owner = owner;
        _body = owner.Body;
        _teamObject = owner.GetComponent<TeamObject>();
    }

    /// <summary>
    /// 当たり判定ON
    /// </summary>
    public virtual void HitOn()
    {
        // 攻撃2回目以降の際にその前に使った内容をクリアするため
        _hitTargets.Clear();
    }


    /// <summary>
    /// 当たり判定OFF
    /// </summary>
    public virtual void HitOff()
    {

    }

    /// <summary>
    /// 攻撃が当たった時の処理
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnHit(Collider other)
    {
        // 自分は無視
        if (other.transform.IsChildOf(_owner?.transform)) return;

        // 攻撃が当たったオブジェクトの陣営を取得
        TeamObject target = other.transform.GetComponentInParent<TeamObject>();

        // 味方は無視
        if (target != null && target.GetTeamType() == _teamObject.GetTeamType()) return;

        // 多段Hitを防止するため当たったオブジェクトの親オブジェクトを参照
        GameObject root = other.transform.root.gameObject;

        if (_hitTargets.Contains(root)) return;

        _hitTargets.Add(root);

        ApplyDamage(other);
    }

    /// <summary>
    /// 攻撃の処理
    /// </summary>
    /// <param name="other"></param>
    protected virtual void ApplyDamage(Collider other)
    {
        if(other.GetComponentInParent<PlayerHP>() is PlayerHP playerHP)
        {
            playerHP.Damage(AttackDamage, Attacker);
        }
        if(other.GetComponentInParent<RobotHPManager>() is RobotHPManager robotHP)
        {
            robotHP.ApplyTotalDamage(AttackDamage, Attacker);
        }
    }
}
