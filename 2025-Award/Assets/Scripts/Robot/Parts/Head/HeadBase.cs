using UnityEngine;
using UnityEngine.AI;

public abstract class HeadBase: MonoBehaviour
{
    [Header("HeadData")]
    public HeadData HeadData { get; set; }

    [Header("Search Settings")]
    [SerializeField] protected float searchRadius = 10f;
    [SerializeField] protected float viewDistance = 15f;
    [SerializeField] protected float viewAngle = 60f;

    [Header("Lose Target")]
    [SerializeField] protected float loseTargetTime = 2.0f;
    protected float loseTimer = 0f;

    // 敵の移動範囲
    protected NavMeshAgent area;
    protected Robot _robot;

    public bool IsPatrolling;

    public abstract void Init();

    /// <summary>
    /// Idle状態ですること
    /// 索敵中にすること
    /// </summary>
    public abstract void TrackingTarget();

    /// <summary>
    /// 敵を追尾する処理
    /// </summary>
    public abstract void ChaseTarget();

    /// <summary>
    /// ロボットができたときに行う初期設定
    /// </summary>
    /// <param name="rig"></param>
    /// <param name="agent"></param>
    public abstract void SetupRig(GameObject rig, NavMeshAgent agent);

    /// <summary>
    /// ロボットがプレイヤーによって作成されたときの初期設定
    /// </summary>
    public abstract void CreateSetup();

    // ここから下は変わらない処理

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual HeadData OutputData()
    {
        return HeadData;
    }

    /// <summary>
    /// 敵を索敵
    /// いるならTrue、いないならFalseにする
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public virtual bool SearchTarget(LayerMask category)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, searchRadius, category);

        float nearestDist = searchRadius;
        Transform nearestTarget = null;

        foreach (var col in cols)
        {
            Vector3 dir = col.transform.position - transform.position;
            float dist = dir.magnitude;

            // 視野角チェック
            if (Vector3.Angle(transform.forward, dir) > viewAngle * 0.5f)
                continue;

            // 壁チェック
            if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, dist))
                if (hit.collider.transform != col.transform)
                    continue;

            // 最も近いターゲット更新
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestTarget = col.transform;
            }
        }

        if (nearestTarget != null)
        {
            loseTimer = 0f;
            _robot.Target = nearestTarget.gameObject;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 追跡対象がいるのか識別
    /// いるならTrue、いないなら一定時間後Falseにする
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckLose()
    {
        if (_robot.Target == null)
            return false;

        loseTimer += Time.deltaTime;

        if (loseTimer > loseTargetTime)
        {
            _robot.Target = null;
            return false;
        }

        return true;
    }
}
