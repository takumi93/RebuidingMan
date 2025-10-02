using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static HeadController;
using static RobotAction;

public class KnightHead : MonoBehaviour, HeadController
{
    [SerializeField] public HeadData headData;

    [Header("胴に連動するRigを指定")]
    [SerializeField] public GameObject HeadToBodyRig = null;
    // 頭が胴に連動するRig
    public GameObject BodyToHeadRig = null;

    public bool IsSet;

    // 追尾対象
    public GameObject Target;

    // 敵の移動範囲
    public NavMeshAgent area;

    // 索敵距離
    private float startDistance;

    // 敵との距離
    private float DistanceHostile;

    [SerializeField]
    public RaycastHit[] hits;

    public RobotAction robotAction;

    private GameObject[] AllyRobots = null;

    // 護衛対象
    public GameObject EscortTarget;

    private float allyDistance;
    private float enemyDistance;
    // ロボットの距離
    private float robotDistance;

    private GameObject PlayerOb = null;

    // キャタピラ時の角度
    const float Fov = 30.0f;

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        area = this.GetComponentInParent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        robotAction = this.GetComponentInParent<RobotAction>();
    }

    public void CreateSetup()
    {
        this.transform.GetComponentInChildren<SkinnedMeshRenderer>().material = headData.material;
    }

    public void PartsDestroy(bool isSet)
    {
        if (isSet)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        else
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    public HeadData OutputData()
    {
        return headData;
    }

    public void SetupRig(GameObject rig, NavMeshAgent agent)
    {
        BodyToHeadRig = rig;
        area = agent;
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
                robotAction.robotState = RobotState.Chase;
                robotAction.Target = hit.transform.gameObject;
                Target = robotAction.Target;
                startDistance = DistanceHostile;
            }
        }
    }

    public void ChaseTarget()
    {
        // ターゲットがいるときは追尾
        if (Target)
        {
            area.isStopped = false;
            Vector3 targetDir = Target.transform.position - transform.position;

            // その方向に向けて旋回する(120度/秒)
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            this.transform.parent.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

            // 自分の向きと次の位置の角度差が30度以上の場合、その場で旋回
            float angle = Vector3.Angle(targetDir, transform.forward);
            if (angle < Fov)
            {
                area.destination = Target.transform.position;
            }
        }
        // いない時は巡回するためにステートを変更
        else
        {
            robotAction.robotState = RobotState.Idle;
        }
    }

    // ================================================================================
    // 変数名: TrackingTarget
    // 索敵中と索敵中にすること
    // ロボットの頭ごとに動きが異なる
    // category:    索敵する対象名
    // ================================================================================
    // 索敵中と索敵中にすること
    public void TrackingTarget(string category)
    {

        // ナイトの時は味方と敵で処理が異なる
        // 敵の時は護衛対象を探し一番近い敵を護衛対象とする
        // 味方の時はプレイヤーを護衛対象とする
        // 護衛対象がいないとき
        if (!EscortTarget)
        {
            // ロボットが敵の時
            if (this.transform.parent.CompareTag("Enemy"))
            {
                // 一番近い味方の距離情報をリセット
                robotDistance = Mathf.Infinity;
                // 敵のロボットを取得（Enemyタグを検索して配列に格納）
                AllyRobots = GameObject.FindGameObjectsWithTag("Enemy");
                // 見つからない場合は処理しない
                if (AllyRobots != null || AllyRobots.Length != 0)
                {
                    // 敵の数分繰り返す
                    foreach (GameObject robot in AllyRobots)
                    {
                        // 体の部位ごとに必要な情報を取得
                        for (int i = 0; i < robot.transform.childCount; i++)
                        {
                            // オブジェクトのTagが頭の時
                            if (robot.transform.GetChild(i).gameObject.CompareTag("Head"))
                            {
                                // 自分を護衛対象する場合を割けるためナイトをのぞく
                                if (robot.transform.GetChild(i).name == this.transform.name)
                                {
                                    // 自分と味方の距離を計測
                                    allyDistance = Vector3.Distance(this.transform.position, robot.transform.position);
                                    // 一番近い味方を護衛対象とする
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
            // 味方の時
            else
            {
                PlayerOb = GameObject.FindGameObjectWithTag("Player");
                EscortTarget = PlayerOb.transform.gameObject;
            }
        }
        else
        {
            if (!Target)
            {
                SearchTarget(headData.radius, headData.distance, category);
                area.destination = EscortTarget.transform.position;
            }
            else
            {
                robotAction.robotState = RobotState.Chase;
            }
        }
    }

    public void ResetTarget()
    {
        Target = null;
    }
}
