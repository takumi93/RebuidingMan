using UnityEngine;

public abstract class BodyBase: PartBase
{
    public BodyData BodyData {  get; private set; }

    [Header("頭に連動するRigを指定")]
    [SerializeField] public GameObject BodyToHeadRig = null;
    [Header("足に連動するRigを指定")]
    [SerializeField] public GameObject BodyToLegRig = null;

    public float lapseTime { get; protected set; }

    public float currentCoolTime { get; protected set; }

    public bool IsAttacking {  get; protected set; }

    public bool IsAttackable { get; protected set; }

    public int Damage { get; protected set; }

    public float AttackRange { get; protected set; }

    public GameObject ConnectRig { get; protected set; }

    public RobotAnimation Animation { get; protected set; }

    public AudioSource audioSource { get; protected set; }

    public WeaponBase Weapon { get; protected set; }

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(Robot robot)
    {
        base.Init(robot);

        Animation = GetComponentInChildren<RobotAnimation>();
        audioSource = robot.GetComponent<AudioSource>();

        Animation.InitBody(this);

        IsAttackable = true;
        lapseTime = 0.0f;
    }

    protected virtual void LateUpdate()
    {
        if (ConnectRig)
        {
            BodyToLegRig.transform.position = ConnectRig.transform.position;
        }
    }

    /// <summary>
    /// 攻撃A
    /// </summary>
    public abstract void AttackA();

    /// <summary>
    /// 攻撃B
    /// </summary>
    public abstract void AttackB();
    
    /// <summary>
    /// 攻撃Aで使用する発射イベント
    /// </summary>
    public virtual void AttackAEvent()
    {

    }

    /// <summary>
    /// 攻撃Bで使用する発射イベント
    /// </summary>
    public virtual void AttackBEvent()
    {

    }

    /// <summary>
    /// データの登録
    /// </summary>
    /// <param name="data"></param>
    public void SetData(BodyData data)
    {
        BodyData = data;
    }

    /// <summary>
    /// クールタイムの更新処理
    /// </summary>
    public void UpdateCoolTime()
    {
        if (IsAttacking) return;

        if (IsAttackable) return;

        lapseTime += Time.deltaTime;

        if (lapseTime >= currentCoolTime)
        {
            IsAttackable = true;
        }
    }

    /// <summary>
    /// 攻撃の開始
    /// </summary>
    public virtual void OnAttackStart()
    {
        IsAttacking = true;
        IsAttackable = false;
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
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

    /// <summary>
    /// 攻撃の終わり
    /// </summary>
    public virtual void OnAttackEnd()
    {
        IsAttacking = false;
        lapseTime = 0f;
    }
}
