using UnityEngine;

public class RobotAttackSlashAxe : MonoBehaviour
{
    [SerializeField]
    BoxCollider Slashcollider;

    PlayerHP playerHp = null;
    RobotHPManager robotAllyHp = null;

    RobotAttackMethod robotAttackMethod = null;

    int SlashDamage;

    void Start()
    {
        Slashcollider.enabled = false;

        //このロボットが敵か味方かを判定
        robotAttackMethod = this.transform.parent.GetComponent<RobotAttackMethod>();

        SlashDamage = robotAttackMethod.axeDamageA;
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
                playerHp.Damage(SlashDamage);
            }
            if (other.transform.parent.CompareTag("Ally"))
            {
                robotAllyHp = other.transform.parent.GetComponent<RobotHPManager>();
                robotAllyHp.HitDamage(SlashDamage);
            }
        }
        //味方の時
        else
        {
            //ロボットのhpコンポーネントを取得。
            if (other.transform.parent.CompareTag("Enemy"))
            {
                robotAllyHp = other.transform.parent.GetComponent<RobotHPManager>();
                robotAllyHp.HitDamage(SlashDamage);
            }
        }
    }

    public void OnCollider()
    {
        Slashcollider.enabled = true;
    }
    public void OffCollider()
    {
        Slashcollider.enabled = false;
    }
}
