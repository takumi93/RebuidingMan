using UnityEngine;
using UnityEngine.AI;

public class PawnHead : HeadBase
{
    [Header("胴に連動するRigを指定")]
    [SerializeField] public GameObject HeadToBodyRig = null;
    // 頭が胴に連動するRig
    public GameObject BodyToHeadRig = null;

    public Transform[] crawlPosition = null;

    // 巡回するポイントの個数
    private int crawlLength = 0;

    // キャタピラ時の角度
    const float moveStartAngle = 30.0f;

    const float rotateSpeed = 120f;

    public override void Init()
    {
        area = this.GetComponentInParent<NavMeshAgent>();
        _robot = this.GetComponentInParent<Robot>();
        IsPatrolling = true;
    }

    // 味方になった時のセットアップ
    public override void CreateSetup()
    {
        // 味方用にマテリアルを変更
        this.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = HeadData.material;
        this.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = HeadData.material;

        crawlPosition = new Transform[StageScene.Instance.AllycrawlPositions.Length];
        // 巡回ルートをステージシーンから持ってくる
        for (int i = 0; i < StageScene.Instance.AllycrawlPositions.Length; i++)
        {
            crawlPosition[i] = StageScene.Instance.AllycrawlPositions[i];
        }
    }

    public override void SetupRig(GameObject rig, NavMeshAgent agent)
    {
        BodyToHeadRig = rig;
        area = agent;
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

    /// <summary>
    /// ロボットが敵を追尾する
    /// </summary>
    public override void ChaseTarget()
    {
        if (_robot.Target == null) return;

        area.isStopped = false;

        Vector3 targetDir = _robot.Target.transform.position - transform.position;
        targetDir.y = 0f; // 上下方向は無視して水平だけで追尾

        // 旋回
        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);

        Transform body = transform.parent; // 回転させたい本体
        body.rotation = Quaternion.RotateTowards(
            body.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );

        // 自分の向きとターゲット方向の角度差
        float angle = Vector3.Angle(body.forward, targetDir);

        if (angle < moveStartAngle)
        {
            area.isStopped = false;
            area.destination = _robot.Target.transform.position;
        }
        else
        {
            area.isStopped = true; // 角度差大きい場合はその場で旋回
        }
    }

    /// <summary>
    /// Idle状態にすること
    /// 索敵中にすること
    /// 敵を索敵と巡回をする
    /// </summary>
    /// <param name="category"></param>
    public override void TrackingTarget()
    {
        if (!area.pathPending && area.remainingDistance < 3.0f)
        {
            TrackingCrawl();
        }
    }
}
