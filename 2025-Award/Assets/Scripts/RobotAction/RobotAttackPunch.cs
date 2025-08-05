using System.Collections.Generic;
using UnityEngine;

public class RobotAttackPunch : MonoBehaviour
{
    [SerializeField]
    new BoxCollider collider;

    PlayerHP playerHp = null;
    RobotHPManager robotAllyHp = null;

    RobotAttackMethod robotAttackMethod = null;

    int PunchDamage;

    void Start()
    {
        collider.enabled = false;

        //このロボットが敵か味方かを判定
        robotAttackMethod = this.transform.parent.GetComponent<RobotAttackMethod>();

        PunchDamage = robotAttackMethod.normalDamageA;
    }

    private void OnTriggerEnter(Collider other)
    {
        //敵の時
        if (robotAttackMethod.isEnemy)
        {
            //コライダーを通ったゲームオブジェクトがプレイヤーか味方の時処理をする。
            if (other.CompareTag("Player"))
            {
                //プレイヤーのhpコンポーネントを取得
                playerHp = other.GetComponent<PlayerHP>();
                playerHp.Damage(PunchDamage);
            }
            else if (other.transform.parent.CompareTag("Ally"))
            {
                robotAllyHp = other.transform.parent.GetComponent<RobotHPManager>();
                robotAllyHp.HitDamage(PunchDamage);
            }
            else
            {
                return;
            }
        }
        //味方の時
        else
        {
            //ロボットのhpコンポーネントを取得。
            if (other.transform.parent.CompareTag("Enemy"))
            {
                robotAllyHp = other.transform.parent.GetComponent<RobotHPManager>();
                robotAllyHp.HitDamage(PunchDamage);
            }
        }
    }

    public void OnCollider()
    {
        collider.enabled = true;
    }
    public void OffCollider()
    {
        collider.enabled = false;
    }
}
