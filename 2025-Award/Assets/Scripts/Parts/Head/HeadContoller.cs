using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static RobotAction;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public interface HeadController
{
    void ChaseTarget();
    void TrackingTarget(string category);

    void ResetTarget();

    void SetupRig(GameObject rig, NavMeshAgent agent);

    HeadData OutputData();

    void PartsDestroy(bool isSet);

    void CreateSetup();
}


//public class HeadController : MonoBehaviour
//{
//    [Header("胴に連動するRigを指定")]
//    [SerializeField] public GameObject HeadToBodyRig = null;
//    // 頭が胴に連動するRig
//    public GameObject BodyToHeadRig = null;

//    public PartsData partsData;

//    const int PawnId = 0;
//    const int RookId = 1;
//    const int KnightId = 2;

//    public bool IsSet;

//    public Transform[] crawlPosition = null;

//    // 巡回するポイントの個数
//    private int crawlLength = 0;

//    // 追尾対象
//    public GameObject Target;

//    // 敵の移動範囲
//    public NavMeshAgent agent;

//    // 索敵距離
//    private float startDistance;

//    // 敵との距離
//    private float DistanceHostile;

//    [SerializeField]
//    public RaycastHit[] hits;

//    public RobotAction robotAction;

//    public Transform FixedPosition = null;

//    private GameObject[] AllyRobots = null;

//    // 護衛対象
//    public GameObject EscortTarget;

//    private float allyDistance;
//    private float enemyDistance;
//    // ロボットの距離
//    private float robotDistance;

//    private GameObject PlayerOb = null;

//    // キャタピラ時の角度
//    const float Fov = 30.0f;

//    public enum HeadState
//    {
//        // 役職なし
//        None,
//        // ポーン
//        Pawn,
//        // ルーク
//        Rook,
//        // ナイト
//        Knight
//    }
//    public HeadState headState = HeadState.None;

//    private void Start()
//    {
//        //// ステートを更新。
//        //if (headDatas.id == PawnId)
//        //{
//        //    headState = HeadState.Pawn;
//        //}
//        //else if (headDatas.id == RookId)
//        //{
//        //    headState = HeadState.Rook;
//        //}
//        //else
//        //{
//        //    headState = HeadState.Knight;
//        //}
//        // ターゲットは親の方から入力するから大丈夫

//    }

//    private void Update()
//    {
//        //// ロボットになっている場合
//        //if (robotAction.IsSet)
//        //{
//        //    this.GetComponent<Rigidbody>().useGravity = false;
//        //    this.GetComponent<Rigidbody>().isKinematic = true;

//        //    if (BodyToHeadRig)
//        //    {
//        //        // 胴体と頭のRigの接合部を同じにすることでアニメーションで頭が動くようにするため
//        //        HeadToBodyRig.transform.position = BodyToHeadRig.transform.position;
//        //    }
//        //}
//        //else
//        //{
//        //    this.GetComponent<Rigidbody>().useGravity = true;
//        //    this.GetComponent<Rigidbody>().isKinematic = false;
//        //    Target = null;
//        //}
//    }

//    //// 特定のポイントを巡回します
//    //public void TrackingCrawl()
//    //{
//    //    // 地点がなにも設定されていないときに返します
//    //    if (crawlPosition == null)
//    //    {
//    //        return;
//    //    }
//    //    else
//    //    {
//    //        if (crawlPosition.Length == 0)
//    //        {
//    //            return;
//    //        }
//    //        // ロボットが止まっているのを解除
//    //        agent.isStopped = false;
//    //        // ロボットが現在設定された目標地点に行くように設定
//    //        agent.destination = crawlPosition[crawlLength].position;

//    //        // 配列内の次の位置を目標地点に設定し、
//    //        // 必要ならば出発地点にもどる
//    //        crawlLength = (crawlLength + 1) % crawlPosition.Length;
//    //    }
//    //}

//    //// ================================================================================
//    //// 変数名: ChaseTarget
//    //// ロボットが敵対行動をしている時の動き方
//    //// ================================================================================
//    //public void ChaseTarget()
//    //{
//    //    switch (headState)
//    //    {
//    //        // 役職なしの時は処理をしない
//    //        case HeadState.None:
//    //            break;

//    //        // ポーンの場合はターゲットがいるときは追尾、いないときは巡回
//    //        case HeadState.Pawn:

//    //            // ターゲットがいるときは追尾
//    //            if (Target)
//    //            {
//    //                agent.isStopped = false;
//    //                Vector3 targetDir = Target.transform.position - transform.position;

//    //                // その方向に向けて旋回する(120度/秒)
//    //                Quaternion targetRotation = Quaternion.LookRotation(targetDir);
//    //                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

//    //                // 自分の向きと次の位置の角度差が30度以上の場合、その場で旋回
//    //                float angle = Vector3.Angle(targetDir, transform.forward);
//    //                if (angle < Fov)
//    //                {
//    //                    agent.destination = Target.transform.position;
//    //                }
//    //            }
//    //            // いない時は巡回するためにステートを変更
//    //            else
//    //            {
//    //                robotAction.robotState = RobotState.Idle;
//    //            }
//    //            break;

//    //        // ルークの時はターゲットがいるときは追尾、いないときは護衛場所に戻る
//    //        case HeadState.Rook:
//    //            var Distance = Vector3.Distance(FixedPosition.position, this.transform.position);
//    //            if (Distance < 5.0f)
//    //            {
//    //                // ターゲットがいるときは追尾
//    //                if (Target)
//    //                {
//    //                    agent.isStopped = false;
//    //                    Vector3 targetDir = Target.transform.position - transform.position;

//    //                    // その方向に向けて旋回する(120度/秒)
//    //                    Quaternion targetRotation = Quaternion.LookRotation(targetDir);
//    //                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

//    //                    // 自分の向きと次の位置の角度差が30度以上の場合、その場で旋回
//    //                    float angle = Vector3.Angle(targetDir, transform.forward);
//    //                    if (angle < Fov)
//    //                    {
//    //                        agent.destination = Target.transform.position;
//    //                    }
//    //                }
//    //                // いない時は巡回するためにステートを変更
//    //                else
//    //                {
//    //                    robotAction.robotState = RobotState.Idle;
//    //                }
//    //            }
//    //            else
//    //            {
//    //                robotAction.robotState = RobotState.Idle;
//    //                Target = null;
//    //            }
//    //            break;

//    //        // ナイトの時は味方と敵で処理が異なる
//    //        // 敵の時は護衛対象を探し一番近い敵を護衛対象とする
//    //        // 味方の時はプレイヤーを護衛対象とする
//    //        case HeadState.Knight:
//    //            // ターゲットがいるときは追尾
//    //            if (Target)
//    //            {
//    //                agent.isStopped = false;
//    //                Vector3 targetDir = Target.transform.position - transform.position;

//    //                // その方向に向けて旋回する(120度/秒)
//    //                Quaternion targetRotation = Quaternion.LookRotation(targetDir);
//    //                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

//    //                // 自分の向きと次の位置の角度差が30度以上の場合、その場で旋回
//    //                float angle = Vector3.Angle(targetDir, transform.forward);
//    //                if (angle < Fov)
//    //                {
//    //                    agent.destination = Target.transform.position;
//    //                }
//    //            }
//    //            // いない時は巡回するためにステートを変更
//    //            else
//    //            {
//    //                robotAction.robotState = RobotState.Idle;
//    //            }
//    //            break;
//    //    }
//    //}

//    //// ================================================================================
//    //// 変数名: TrackingTarget
//    //// 索敵中と索敵中にすること
//    //// ロボットの頭ごとに動きが異なる
//    //// category:    索敵する対象名
//    //// ================================================================================
//    //// 索敵中と索敵中にすること
//    //public void TrackingTarget(string category)
//    //{
//    //    // ロボットの頭の状態により変化
//    //    switch (headState)
//    //    {
//    //        // 役職なしの時は処理をしない
//    //        case HeadState.None:
//    //            break;

//    //        // ポーンの場合はターゲットがいるときは追尾、いないときは巡回
//    //        case HeadState.Pawn:

//    //            // ターゲットがないとき
//    //            if (!Target)
//    //            {
//    //                SearchTarget(partsData.radius, partsData.distance, category);
//    //                if (!agent.pathPending && agent.remainingDistance < 3.0f)
//    //                {
//    //                    TrackingCrawl();
//    //                }
//    //            }
//    //            else
//    //            {
//    //                robotAction.robotState = RobotState.Chase;
//    //            }
//    //            break;

//    //        // ルークの時はターゲットがいるときは追尾、いないときは護衛場所に戻る
//    //        case HeadState.Rook:
//    //            var FixedDistance = Vector3.Distance(FixedPosition.position, this.transform.position);
//    //            if (FixedDistance <= 5.0f)
//    //            {
//    //                if (!Target)
//    //                {
//    //                    SearchTarget(partsData.radius, partsData.distance, category);
//    //                }
//    //                else
//    //                {
//    //                    robotAction.robotState = RobotState.Chase;
//    //                }
//    //            }
//    //            else
//    //            {
//    //                agent.destination = FixedPosition.position;
//    //            }
//    //            break;

//    //        // ナイトの時は味方と敵で処理が異なる
//    //        // 敵の時は護衛対象を探し一番近い敵を護衛対象とする
//    //        // 味方の時はプレイヤーを護衛対象とする
//    //        case HeadState.Knight:
//    //            // 護衛対象がいないとき
//    //            if (!EscortTarget)
//    //            {
//    //                // ロボットが敵の時
//    //                if (this.transform.parent.CompareTag("Enemy"))
//    //                {
//    //                    // 一番近い味方の距離情報をリセット
//    //                    robotDistance = Mathf.Infinity;
//    //                    // 敵のロボットを取得（Enemyタグを検索して配列に格納）
//    //                    AllyRobots = GameObject.FindGameObjectsWithTag("Enemy");
//    //                    // 見つからない場合は処理しない
//    //                    if (AllyRobots != null || AllyRobots.Length != 0)
//    //                    {
//    //                        // 敵の数分繰り返す
//    //                        foreach (GameObject robot in AllyRobots)
//    //                        {
//    //                            // 体の部位ごとに必要な情報を取得
//    //                            for (int i = 0; i < robot.transform.childCount; i++)
//    //                            {
//    //                                // オブジェクトのTagが頭の時
//    //                                if (robot.transform.GetChild(i).gameObject.CompareTag("Head"))
//    //                                {
//    //                                    // 自分を護衛対象する場合を割けるためナイトをのぞく
//    //                                    if (robot.transform.GetChild(i).name == this.transform.name)
//    //                                    {
//    //                                        // 自分と味方の距離を計測
//    //                                        allyDistance = Vector3.Distance(this.transform.position, robot.transform.position);
//    //                                        // 一番近い味方を護衛対象とする
//    //                                        if (allyDistance < robotDistance)
//    //                                        {
//    //                                            EscortTarget = robot.transform.gameObject;
//    //                                            robotDistance = allyDistance;
//    //                                        }
//    //                                    }
//    //                                }
//    //                            }
//    //                        }
//    //                    }
//    //                }
//    //                // 味方の時
//    //                else
//    //                {
//    //                    PlayerOb = GameObject.FindGameObjectWithTag("Player");
//    //                    EscortTarget = PlayerOb.transform.gameObject;
//    //                }
//    //            }
//    //            else
//    //            {
//    //                if (!Target)
//    //                {
//    //                    SearchTarget(partsData.radius, partsData.distance, category);
//    //                    agent.destination = EscortTarget.transform.position;
//    //                }
//    //                else
//    //                {
//    //                    robotAction.robotState = RobotState.Chase;
//    //                }
//    //            }
//    //            break;
//    //    }
//    //}
//}
