using System.Collections.Generic;
using UnityEngine;

public class RobotAttackA : MonoBehaviour
{
    PlayerHP playerHp = null;
    RobotHPManager robotAllyHp = null;

    BodyBase body = null;

    RobotAction robotAction = null;

    int DamageA;

    void Start()
    {
        //このロボットが敵か味方かを判定
        body = this.transform.parent.GetComponent<BodyBase>();

        robotAction = this.transform.GetComponentInParent<RobotAction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //敵の時
        if (robotAction.IsEnemy)
        {
            //コライダーを通ったゲームオブジェクトがプレイヤーか味方の時処理をする。
            if (other.CompareTag("Player"))
            {
                //プレイヤーのhpコンポーネントを取得
                playerHp = other.GetComponent<PlayerHP>();
                //playerHp.Damage(DamageA);
            }
            else if (other.transform.parent.CompareTag("Ally"))
            {
                robotAllyHp = other.transform.parent.GetComponent<RobotHPManager>();
                //robotAllyHp.HitDamage(DamageA);
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
                //robotAllyHp.HitDamage(DamageA);
            }
        }
    }
}
