using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected BodyBase _body;
    protected TeamObject _teamObject;
    protected Robot _robot;

    // 武器に当たった判定を残すための変数
    private HashSet<GameObject> _hitTargets = new HashSet<GameObject>();

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Init()
    {
        _body = GetComponentInParent<BodyBase>();
        _teamObject = GetComponentInParent<TeamObject>();
        _robot = GetComponentInParent<Robot>();
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
        if (other.transform.IsChildOf(_robot?.transform)) return;

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
            playerHP.Damage(_body.Damage, _robot.gameObject);
        }
        if(other.GetComponentInParent<RobotHPManager>() is  RobotHPManager robotHP)
        {
            robotHP.ApplyTotalDamage(_body.Damage, _robot.gameObject);
        }
    }
}
