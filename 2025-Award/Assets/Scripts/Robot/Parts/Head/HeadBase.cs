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

    [Header("胴に連動するRigを指定")]
    [SerializeField] public GameObject HeadToBodyRig = null;
    // 頭が胴に連動するRig
    public GameObject BodyToHeadRig = null;

    // キャタピラ時の角度
    const float moveStartAngle = 30.0f;

    const float rotateSpeed = 120f;

    // 敵の移動範囲
    protected NavMeshAgent _area;
    protected Robot _robot;

    public bool IsPatrolling;

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Init()
    {
        _area = GetComponentInParent<NavMeshAgent>();
        _robot = GetComponentInParent<Robot>();
    }

    /// <summary>
    /// プレイヤー陣営になった時マテリアルを変更する
    /// </summary>
    protected void UpdateMaterial()
    {
        var renderer = GetComponentInChildren<Renderer>();

        if (renderer != null)
        {
            renderer.material = HeadData.AllyMaterial;
        }
    }

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
    /// 移動先を指定
    /// </summary>
    /// <param name="targetPos"></param>
    public void MoveToTarget(Vector3 targetPos)
    {
        _area.isStopped = false;
        _area.destination = targetPos;
    }

    ///// <summary>
    ///// ロボットができたときに行う初期設定
    ///// </summary>
    ///// <param name="rig"></param>
    ///// <param name="agent"></param>
    //public abstract void SetupRig(GameObject rig, NavMeshAgent agent);

    /// <summary>
    /// ロボットがプレイヤーによって作成されたときの初期設定
    /// </summary>
    public abstract void CreateSetup();

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

    //public virtual void SetupRig(GameObject rig, NavMeshAgent agent)
    //{
    //    BodyToHeadRig = rig;
    //    _area = agent;
    //}
}
