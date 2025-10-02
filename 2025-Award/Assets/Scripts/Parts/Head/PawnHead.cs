using UnityEngine;
using UnityEngine.AI;
using static RobotAction;

public class PawnHead : MonoBehaviour, HeadController
{
    [SerializeField]public HeadData headData;

    [Header("胴に連動するRigを指定")]
    [SerializeField] public GameObject HeadToBodyRig = null;
    // 頭が胴に連動するRig
    public GameObject BodyToHeadRig = null;

    public bool IsSet;

    public Transform[] crawlPosition = null;

    // 巡回するポイントの個数
    private int crawlLength = 0;

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

    private float allyDistance;
    private float enemyDistance;
    // ロボットの距離
    private float robotDistance;

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

    // 味方になった時のセットアップ
    public void CreateSetup()
    {
        // 味方用にマテリアルを変更
        this.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = headData.material;
        this.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = headData.material;

        crawlPosition = new Transform[StageScene.Instance.AllycrawlPositions.Length];
        // 巡回ルートをステージシーンから持ってくる
        for (int i = 0; i < StageScene.Instance.AllycrawlPositions.Length; i++)
        {
            crawlPosition[i] = StageScene.Instance.AllycrawlPositions[i];
        }
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

    // 特定のポイントを巡回します
    public void TrackingCrawl()
    {
        // 地点がなにも設定されていないときに返します
        if (crawlPosition == null)
        {
            return;
        }
        else
        {
            if (crawlPosition.Length == 0)
            {
                return;
            }
            // ロボットが止まっているのを解除
            area.isStopped = false;
            // ロボットが現在設定された目標地点に行くように設定
            area.destination = crawlPosition[crawlLength].position;

            // 配列内の次の位置を目標地点に設定し、
            // 必要ならば出発地点にもどる
            crawlLength = (crawlLength + 1) % crawlPosition.Length;
        }
    }

    // ================================================================================
    // 変数名: ChaseTarget
    // ロボットが敵対行動をしている時の動き方
    // ================================================================================
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
        // ターゲットがないとき
        if (!Target)
        {
            SearchTarget(headData.radius, headData.distance, category);
            if (!area.pathPending && area.remainingDistance < 3.0f)
            {
                TrackingCrawl();
            }
        }
        else
        {
            robotAction.robotState = RobotState.Chase;
        }
    }

    public void ResetTarget()
    {
        Target = null;
    }
}
