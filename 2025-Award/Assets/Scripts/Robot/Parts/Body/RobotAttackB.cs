using UnityEngine;

public class RobotAttackB : MonoBehaviour
{
    PlayerHP playerHp = null;
    RobotHPManager robotAllyHp = null;

    BodyBase body = null;

    RobotAction robotAction = null;

    int DamageB;

    void Start()
    {
        //このロボットが敵か味方かを判定
        body = this.transform.parent.GetComponent<BodyBase>();

        robotAction = this.transform.GetComponentInParent<RobotAction>();

        DamageB = body.GetDamageB();
    }

    private void OnTriggerEnter(Collider other)
    {
        //敵ロボットの時
        if (robotAction.IsEnemy)
        {
            //コライダーを通ったゲームオブジェクトがプレイヤーか味方の時処理をする。
            if (other.CompareTag("Player"))
            {
                //プレイヤーのhpコンポーネントを取得
                playerHp = other.GetComponent<PlayerHP>();
                //playerHp.Damage(DamageB);
            }
            else if (other.transform.parent.CompareTag("Ally"))
            {
                robotAllyHp = other.transform.parent.GetComponent<RobotHPManager>();
                //robotAllyHp.HitDamage(DamageB);
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
                //robotAllyHp.HitDamage(DamageB);
            }
        }
    }
}
