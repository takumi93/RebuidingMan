using UnityEngine;
using UnityEngine.AI;

public class RobotAction : MonoBehaviour
{
    // 頭のスクリプトの格納場所を指定
    private HeadBase head;
    // 胴のスクリプトの格納場所を指定
    private BodyBase body;
    // 足のスクリプトの格納場所を指定
    private LegBase leg;

    public RobotHPManager hpManager;

    // 敵の移動範囲
    public NavMeshAgent agent;
    // 追尾対象
    public GameObject Target;

    public float targetDistance;

    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float chaseRange = 10f;



    public bool IsSet = true;

    public bool IsEnemy { get; private set; }

    // ロボットの状態管理
    public enum RobotState
    {
        // 待機状態
        Idle,
        // 追尾状態
        Chase,
        // 攻撃状態
        Attack,
        // 破壊状態
        Destruction,
    }
    public RobotState robotState = RobotState.Idle;

    private void Start()
    {
        // 味方と敵で索敵や攻撃の際に処理を分けるためIsEnemyに結果を格納
        if (this.CompareTag("Ally"))
        {
            IsEnemy = false;
        }
        else if (this.CompareTag("Enemy"))
        {
            IsEnemy = true;
        }

        robotState = RobotState.Idle;

        // 移動範囲の取得
        agent = this.GetComponent<NavMeshAgent>();

        hpManager = this.GetComponent<RobotHPManager>();

        IsSet = true;

        // 体の部位ごとに必要な情報を取得
        //head = this.transform.GetComponentInChildren<IHeadController>();
        body = this.transform.GetComponentInChildren<BodyBase>();
        leg = this.transform.GetComponentInChildren<LegBase>();

        //head.BodyToHeadRig = body.BodyToHeadRig;
        //body.LegToBodyRig = leg.LegToBodyRig;
    }

    private void Update()
    {
        //// 敵と味方で処理を分ける
        //if (IsEnemy)
        //{
        //    allyAction("Player");
        //}
        //else
        //{
        //    allyAction("Enemy");
        //}
    }

    private void allyAction(LayerMask category)
    {
        switch (robotState)
        {
            case RobotState.Idle: HandleIdle(category); break;
            case RobotState.Chase: HandleChase(); break;
            case RobotState.Attack: HandleAttack(); break;
            case RobotState.Destruction: HandleDestruction(); break;
        }
    }

    private void HandleIdle(LayerMask category)
    {
        //head.TrackingTarget(category);
        if (Target != null)
        {
            robotState = RobotState.Chase;
        }
    }

    private void HandleChase()
    {
        if (Target == null)
        {
            robotState = RobotState.Idle;
            return;
        }

        //head.ChaseTarget();

        targetDistance = Vector3.Distance(transform.position, Target.transform.position);

        if (targetDistance < attackRange)
        {
            robotState = RobotState.Attack;
        }
        else if (targetDistance > chaseRange)
        {
            robotState = RobotState.Idle;
        }
    }

    private void HandleAttack()
    {
        if (Target == null)
        {
            robotState = RobotState.Idle;
            return;
        }

        //head.ChaseTarget();
        body.Attack();

        targetDistance = Vector3.Distance(transform.position, Target.transform.position);
        if (targetDistance > attackRange)
        {
            robotState = RobotState.Chase;
        }
    }

    private void HandleDestruction()
    {
        // 破壊処理（死亡アニメーションなど）
    }
}
