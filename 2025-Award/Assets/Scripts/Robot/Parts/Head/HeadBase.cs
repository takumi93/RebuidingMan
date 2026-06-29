using UnityEngine;
using UnityEngine.AI;

public abstract class HeadBase: PartBase
{
    [Header("HeadData")]
    public HeadData HeadData { get; private set; }

    [Header("Search Settings")]
    [SerializeField] protected float searchRadius = 10f;
    [SerializeField] protected float viewDistance = 15f;
    [SerializeField] protected float viewAngle = 60f;

    [Header("Lose Target")]
    [SerializeField] protected float loseTargetTime = 2.0f;
    protected float loseTimer = 0f;

    [Header("胴に連動するRigを指定")]
    [SerializeField] public GameObject HeadToBodyRig = null;

    public bool IsPatrolling;

    ///// <summary>
    ///// 初期化
    ///// </summary>
    //public virtual void Init()
    //{
        
    //}

    /// <summary>
    /// Idle状態ですること
    /// 索敵中にすること
    /// </summary>
    public abstract void TrackingTarget();

    /// <summary>
    /// 敵を見つけたとき移動先を敵にする
    /// </summary>
    public abstract void ChaseTarget();

    public void SetData(HeadData data)
    {
        HeadData = data;
    }


    ///// <summary>
    ///// ロボットができたときに行う初期設定
    ///// </summary>
    ///// <param name="rig"></param>
    ///// <param name="agent"></param>
    //public abstract void SetupRig(GameObject rig, NavMeshAgent agent);

    /// <summary>
    /// 敵を索敵
    /// いるならTrue、いないならFalseにする
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public virtual bool SearchTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, searchRadius);

        // 自分の陣営を確認
        TeamObject myTeam = _robot.GetComponent<TeamObject>();

        float nearestDist = searchRadius;
        Transform nearestTarget = null;

        foreach (var col in cols)
        {
            // 自分の視線の先にあるものの陣営を確認
            TeamObject targetTeam = col.GetComponentInParent<TeamObject>();

            // 陣営を取得できなかったとき
            if(targetTeam == null) continue;
            // 味方のとき
            if(!myTeam.IsEnemy(targetTeam)) continue;

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

            _robot.Target = nearestTarget;
            _robot.MoveTarget = nearestTarget.position;

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
        if (_robot.Target == null) return false;

        Vector3 dir = _robot.Target.position - transform.position;

        float dist = dir.magnitude;

        bool canSee = true;

        // 距離判定
        if(dist > viewDistance) canSee = false;

        // 敵の視界に入っているか判定
        if(Vector3.Angle(transform.forward, dir) > viewAngle * 0.5f) canSee = false;

        // 壁チェック
        if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, dist))
            if (!hit.transform.IsChildOf(_robot.Target))
                canSee = false;

        // 見えてるなら追尾する
        if (canSee)
        {
            loseTimer = 0.0f;

            _robot.MoveTarget = _robot.Target.position;

            return true;
        }

        loseTimer += Time.deltaTime;

        if (loseTimer >= loseTargetTime)
        {
            _robot.Target = null;
            return false;
        }

        return true;
    }

    //public virtual void SetupRig(GameObject rig, NavMeshAgent agent)
    //{
    //    BodyToHeadRig = rig;
    //    _area = agent;
    //}
}
