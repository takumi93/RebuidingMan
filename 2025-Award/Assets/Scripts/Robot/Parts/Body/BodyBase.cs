using UnityEngine;

public abstract class BodyBase: MonoBehaviour
{
    public BodyData BodyData {  get; set; }

    public float lapseTime { get; set; }

    public float currentCoolTime { get; set; }

    public bool IsAttackable { get; set; }

    public int Damage { get; set; }

    public GameObject ConnectRig { get; set; }

    public RobotAnimation Animation { get; set; }

    public AudioSource audioSource { get; set; }

    public abstract void Init();

    public abstract void AttackA();

    public abstract void AttackB();

    public abstract int GetDamageA();

    public abstract int GetDamageB();

    public abstract void CreateSetup();

    public virtual BodyData OutputData()
    {
        return BodyData;
    }

    public virtual void Attack()
    {
        //攻撃中か、クールタイム中なら返す。
        if (!IsAttackable)
        {
            return;
        }
        //違うなら攻撃中をtrueにして処理を実行
        else
        {
            float r = Random.value;

            if (r < 0.3f)  // 30%でA
            {
                AttackA();
            }
            else           // 70%でB
            {
                AttackB();
            }
        }
    }
}
