using UnityEngine;
using UnityEngine.AI;

public class RookHead : HeadBase
{
    [Header("胴に連動するRigを指定")]
    [SerializeField] public GameObject HeadToBodyRig;
    // 頭が胴に連動するRig
    public GameObject BodyToHeadRig;

    public Transform FixedPosition;

    // キャタピラ時の角度
    const float moveStartAngle = 30.0f;

    const float rotateSpeed = 120f;

    const float maxDistanceFromFixed = 10.0f;

    public override void Init()
    {
        area = this.GetComponentInParent<NavMeshAgent>();
        _robot = this.GetComponentInParent<Robot>();
        IsPatrolling = false;
    }

    // 味方になった時のセットアップ
    public override void CreateSetup()
    {
        // 味方用にマテリアルを変更
        this.transform.GetComponentInChildren<SkinnedMeshRenderer>().material = HeadData.material;
        var Fix = Instantiate(StageScene.Instance.GuardianPoint, StageScene.Instance.GuardianTransform.transform);
        Fix.name = "GuardianPoint";
        Fix.transform.position = StageScene.Instance.GuardianTransform.transform.position;
        FixedPosition = Fix.transform;
    }

    public override void SetupRig(GameObject rig, NavMeshAgent agent)
    {
        BodyToHeadRig = rig;
        area = agent;
    }

    /// <summary>
    /// 敵を追尾する処理
    /// </summary>
    public override void ChaseTarget()
    {
        if (_robot.Target == null) return;

        // 固定位置からの距離チェック
        float distanceFromFixed = Vector3.Distance(FixedPosition.position, transform.position);
        if (distanceFromFixed > maxDistanceFromFixed)
        {
            // 一定距離以上離れたらターゲット諦める
            _robot.Target = null;
            area.isStopped = false;
            area.destination = FixedPosition.position; // 戻る
            return;
        }

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
    /// </summary>
    /// <param name="category"></param>
    public override void TrackingTarget()
    {

    }
}
