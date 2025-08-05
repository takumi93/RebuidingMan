using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class RobotAction : MonoBehaviour
{
    // 攻撃方法を記載しているスクリプトを指定
    private RobotAttackMethod attackMethod = null;
    // 敵の移動範囲
    private NavMeshAgent agent;
    // 追尾対象
    public GameObject Target;

    // 頭の種類
    public int CategoryHead;
    // 胴の種類
    public int CategoryBody;
    // 足の種類
    public int CategoryLeg;

    // ロボットデータに登録している個体番号
    // ポーンの個体番号
    const int NumberPawn = 0;
    // ルークの個体番号
    const int NumberRook = 1;
    // ナイトの個体番号
    const int NumberKnight = 2;

    // 味方の攻撃範囲レイヤーの指定
    const int LayerNumberShot = 11;
    // 敵の攻撃範囲レイヤーの指定
    const int LayerNumberEnemyShot = 12;

    // 巡回するポイントの個数
    private int crawlLength = 0;

    // EMPオブジェクト
    GameObject Emp;

    [SerializeField]
    public GameObject attack;

    // キャタピラ時の角度
    const float Fov = 30.0f;

    // ロボットがRaycastで取得した情報を格納
    [SerializeField]
    public RaycastHit[] hits;

    // 敵との距離
    private float DistanceHostile;
    // 索敵距離
    private float startDistance;
    // ロボットの距離
    private float robotDistance = 100.0f;

    private float allyDistance;
    private float enemyDistance;

    [Header("Pawn時に巡回する個所を指定")]
    // 巡回ポジション
    [SerializeField] public Transform[] crawlPositions = null;

    [Header("Pawn時のパラメータ")]
    [SerializeField]
    private float pawnRadius = 5.0f;
    [SerializeField]
    private float pawnDistance = 10.0f;

    [Header("Rook時に護衛するポイントを指定")]
    // 固定ポジション
    [SerializeField] public Transform FixedPosition = null;

    [Header("Rook時のパラメータ")]
    [SerializeField]
    private float rookRadius = 5.0f;
    [SerializeField]
    private float rookDistance = 10.0f;

    [Header("Knight時に護衛する対象を指定")]
    // 護衛対象
    public GameObject EscortTarget;
    // 
    private GameObject[] AllyRobots = null;

    private GameObject PlayerOb = null;

    [Header("Knight時のパラメータ")]
    [SerializeField]
    private float knightRadius = 5.0f;
    [SerializeField]
    private float knightDistance = 10.0f;

    HeadRig headRig;
    BodyRig bodyRig;
    LegRig legRig;


    // ロボットの種類
    private enum GroupState
    {
        // 味方
        Ally,
        // 敵
        Enemy,
    }
    GroupState groupState;

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

    void Start()
    {
        // 移動範囲の取得
        agent = this.GetComponent<NavMeshAgent>();

        if (this.CompareTag("Ally"))
        {
            groupState = GroupState.Ally;
        }else if (this.CompareTag("Enemy"))
        {
            groupState = GroupState.Enemy;
        }

        // 指定したポイントを巡回
        //TrackingCrawl();

        // 体の部位ごとに必要な情報を取得
        for(int i = 0; i < transform.childCount; i++)
        {
            // 頭の時
            if (this.transform.GetChild(i).gameObject.layer == 8)
            {
                // 頭と胴体を連動させるためにRigの情報を登録しているスクリプトを取得
                headRig = this.transform.GetChild(i).GetComponent<HeadRig>();
            }
            // 胴の時
            else if (this.transform.GetChild(i).gameObject.layer == 9)
            {
                // 頭と胴体と足を連動させるためにRigの情報を登録しているスクリプトを取得
                bodyRig = this.transform.GetChild(i).GetComponent<BodyRig>();
                // 攻撃方法を記載しているスクリプトを取得
                attackMethod = this.transform.GetChild(i).GetComponent<RobotAttackMethod>();
                //attackMethod = this.transform.GetChild(i).GetChild(1).GetComponent<RobotAttackMethod>();
            }
            // 足の時
            else if(this.transform.GetChild(i).gameObject.layer == 10)
            {
                // 足と胴体を連動させるためにRigの情報を登録しているスクリプトを取得
                legRig = this.transform.GetChild(i).GetComponent<LegRig>();
            }
        }
    }

    private void FixedUpdate()
    {
        // パーツごとに連動するようにRigの座標を固定
        headRig.HeadToBodyRig.transform.position = bodyRig.BodyToHeadRig.transform.position;
        bodyRig.BodyToLegRig.transform.position = legRig.LegToBodyRig.transform.position;
        // 味方と敵で処理を変更
        switch (groupState)
        {
            case GroupState.Ally:
                // ロボットの状態によって変更
                switch (robotState)
                {
                    case RobotState.Idle:
                        // Enemyレイヤーがついてるオブジェクトを追跡
                        TrachingTarget("Enemy");
                        
                        break;
                    case RobotState.Chase:
                        // 追尾ルートがあるとき
                        if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
                        {
                            // 敵の位置を取得し追尾
                            ChaseTarget();
                            if (Target)
                            {
                                enemyDistance = Vector3.Distance(transform.position, Target.transform.position);
                                if (enemyDistance < 3)
                                {
                                    robotState = RobotState.Attack;
                                }else if (enemyDistance > 10)
                                {
                                    robotState = RobotState.Idle;
                                }
                            }
                            else
                            {
                                Target = null;
                                robotState = RobotState.Idle;
                            }
                        }
                        break;
                    case RobotState.Attack:
                        agent.isStopped = true;
                        EnemyAttack();
                        break;
                    case RobotState.Destruction:
                        break;
                    default:
                        break;
                }
                break;

            case GroupState.Enemy:
                switch (robotState)
                {
                    case RobotState.Idle:
                        // Playerレイヤーがついてるオブジェクトを追跡
                        TrachingTarget("Player");
                        break;
                    case RobotState.Chase:
                        // 追尾ルートがあるとき
                        if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
                        {
                            // 敵の位置を取得し追尾
                            ChaseTarget();
                            if (Target)
                            {
                                enemyDistance = Vector3.Distance(transform.position, Target.transform.position);
                                if (enemyDistance < 3.0f)
                                {
                                    robotState = RobotState.Attack;
                                }
                                else if (enemyDistance > 10)
                                {
                                    Target = null;
                                    robotState = RobotState.Idle;
                                }
                            }
                            else
                            {
                                robotState = RobotState.Idle;
                            }
                        }
                        break;
                    case RobotState.Attack:
                        //agent.destination = this.transform.position;
                        agent.isStopped = true;
                        //attackMethod.Attack();
                        EnemyAttack();
                        break;
                    case RobotState.Destruction:
                        break;
                    default:
                        break;
                }
                break;

            default:
                break;
        }
    }

    private void EnemyAttack()
    {
        attackMethod.Attack();
        robotState = RobotState.Idle;
    }

    // 特定のポイントを巡回します
    public void TrackingCrawl()
    {
        // 地点がなにも設定されていないときに返します
        if (crawlPositions == null)
        {
            return;
        }
        else
        {
            if(crawlPositions.Length == 0)
            {
                return;
            }
            // ロボットが止まっているのを解除
            agent.isStopped = false;
            // ロボットが現在設定された目標地点に行くように設定
            agent.destination = crawlPositions[crawlLength].position;

            // 配列内の次の位置を目標地点に設定し、
            // 必要ならば出発地点にもどる
            crawlLength = (crawlLength + 1) % crawlPositions.Length;
        }
    }

    // ================================================================================
    // 変数名: TrachingTarget
    // 索敵中と索敵中にすること
    // ロボットの頭ごとに動きが異なる
    // category:    索敵する対象名
    // ================================================================================
    // 索敵中と索敵中にすること
    public void TrachingTarget(string category)
    {
        // ポーンの時
        if(CategoryHead == NumberPawn)
        {
            if (!Target)
            {
                SearchTarget(pawnRadius, pawnDistance, category);
                if (!agent.pathPending && agent.remainingDistance < 3.0f)
                {
                    TrackingCrawl();
                }
            }
            else
            {
                robotState = RobotState.Chase;
            }
        }
        // ルークの時
        else if (CategoryHead == NumberRook)
        {
            var FixedDistance = Vector3.Distance (FixedPosition.position, this.transform.position);
            if (FixedDistance <= 5.0f)
            {
                if (!Target)
                {
                    SearchTarget(rookRadius, rookDistance, category);
                }
                else
                {
                    robotState = RobotState.Chase;
                }
            }
            else
            {
                agent.destination = FixedPosition.position;
            }
        }
        // ナイトの時
        else
        {
            if(groupState == GroupState.Enemy)
            {
                if (!EscortTarget)
                {
                    // 自分の味方のロボットを取得
                    AllyRobots = GameObject.FindGameObjectsWithTag(gameObject.tag);
                    if (AllyRobots != null || AllyRobots.Length != 0)
                    {
                        foreach (GameObject robot in AllyRobots)
                        {
                            // 体の部位ごとに必要な情報を取得
                            for (int i = 0; i < robot.transform.childCount; i++)
                            {
                                if (robot.transform.GetChild(i).gameObject.layer == 8)
                                {
                                    if (robot.transform.GetChild(i).tag != "Knight")
                                    {
                                        allyDistance = Vector3.Distance(transform.position, robot.transform.position);
                                        if (allyDistance < robotDistance)
                                        {
                                            EscortTarget = robot.transform.gameObject;
                                            robotDistance = allyDistance;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    robotDistance = 100.0f;
                    if (!Target)
                    {
                        SearchTarget(knightRadius, knightDistance, category);
                        agent.destination = EscortTarget.transform.position;
                    }
                    else
                    {
                        robotState = RobotState.Chase;
                    }
                }
            }
            else
            {
                if (!EscortTarget)
                {
                    PlayerOb = GameObject.FindGameObjectWithTag("Player");
                    EscortTarget = PlayerOb.transform.gameObject;
                }
                else
                {
                    robotDistance = 100.0f;
                    if (!Target)
                    {
                        SearchTarget(knightRadius, knightDistance, category);
                        agent.destination = EscortTarget.transform.position;
                    }
                    else
                    {
                        robotState = RobotState.Chase;
                    }
                }
            }


        }
    }

    // ================================================================================
    // 変数名: SearchTarget
    // 敵ロボットを索敵して近い敵をターゲット
    // radius:      SphereCastAllに使う半径
    // maxDistance: SphereCastAllに使う最大距離
    // category:    索敵する対象名
    // ================================================================================
    private void SearchTarget(float radius, float maxDistance, string category)
    {
        startDistance = maxDistance;
        hits = Physics.SphereCastAll(
                        this.transform.position,
                        5.0f,
                        Vector3.forward,
                        10.0f,
                        LayerMask.GetMask(category)
                    );

        foreach (RaycastHit hit in hits)
        {
            DistanceHostile = Vector3.Distance(transform.position, hit.transform.position);
            if (DistanceHostile < startDistance)
            {
                robotState = RobotState.Chase;
                Target = hit.transform.gameObject;
                startDistance = DistanceHostile;
            }
        }
    }


    // ================================================================================
    // 変数名: ChaseTarget
    // ロボットが敵対行動をしている時の動き方
    // ================================================================================
    private void ChaseTarget()
    {
        // ポーンの時
        if (CategoryHead == NumberPawn)
        {
            // ターゲットがいるときは追尾
            if (Target)
            {
                agent.isStopped = false;
                Vector3 targetDir = Target.transform.position - transform.position;

                // その方向に向けて旋回する(120度/秒)
                Quaternion targetRotation = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

                // 自分の向きと次の位置の角度差が30度以上の場合、その場で旋回
                float angle = Vector3.Angle(targetDir, transform.forward);
                if (angle < Fov)
                {
                    agent.destination = Target.transform.position;
                }
            }
            // いない時は巡回するためにステートを変更
            else
            {
                robotState = RobotState.Idle;
            }
        }
        // ルークの時
        else if (CategoryHead == NumberRook)
        {
            var Distance = Vector3.Distance(FixedPosition.position, this.transform.position);
            if(Distance < 5.0f)
            {
                // ターゲットがいるときは追尾
                if (Target)
                {
                    agent.isStopped = false;
                    Vector3 targetDir = Target.transform.position - transform.position;

                    // その方向に向けて旋回する(120度/秒)
                    Quaternion targetRotation = Quaternion.LookRotation(targetDir);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

                    // 自分の向きと次の位置の角度差が30度以上の場合、その場で旋回
                    float angle = Vector3.Angle(targetDir, transform.forward);
                    if (angle < Fov)
                    {
                        agent.destination = Target.transform.position;
                    }
                }
                // いない時は巡回するためにステートを変更
                else
                {
                    robotState = RobotState.Idle;
                }
            }
            else
            {
                robotState = RobotState.Idle;
                Target = null;
            }
        }
        // ナイトの時
        else
        {
            // ターゲットがいるときは追尾
            if (Target)
            {
                agent.isStopped = false;
                Vector3 targetDir = Target.transform.position - transform.position;

                // その方向に向けて旋回する(120度/秒)
                Quaternion targetRotation = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

                // 自分の向きと次の位置の角度差が30度以上の場合、その場で旋回
                float angle = Vector3.Angle(targetDir, transform.forward);
                if (angle < Fov)
                {
                    agent.destination = Target.transform.position;
                }
            }
            // いない時は巡回するためにステートを変更
            else
            {
                robotState = RobotState.Idle;
            }
        }
    }
}
