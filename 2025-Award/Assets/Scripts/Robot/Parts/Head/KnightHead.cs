using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class KnightHead : HeadBase
{
    [Header("胴に連動するRigを指定")]
    [SerializeField] public GameObject HeadToBodyRig = null;
    // 頭が胴に連動するRig
    public GameObject BodyToHeadRig = null;

    // 護衛対象
    public GameObject EscortTarget { get; private set; }

    // キャタピラ時の角度
    const float moveStartAngle = 30.0f;

    const float rotateSpeed = 120f;

    [SerializeField] LayerMask category; // レイヤー名

    public override void Init()
    {
        area = this.GetComponentInParent<NavMeshAgent>();
        _robot = this.GetComponentInParent<Robot>();
        IsPatrolling = true;
    }

    public override void CreateSetup()
    {
        transform.GetComponentInChildren<SkinnedMeshRenderer>().material = HeadData.material;
    }

    public override void SetupRig(GameObject rig, NavMeshAgent agent)
    {
        BodyToHeadRig = rig;
        area = agent;
    }

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
    /// 敵の時は護衛対象を探し一番近い敵を護衛対象とする
    /// 味方の時はプレイヤーを護衛対象とする
    /// </summary>
    public override void TrackingTarget()
    {
        // 護衛対象がいないとき
        if (!EscortTarget)
        {
            // 敵ロボットの場合
            if (transform.parent.CompareTag("Enemy"))
            {
                GameObject[] allies = GameObject.FindGameObjectsWithTag("Enemy");
                EscortTarget = allies
                    .Where(a => a != this.transform.parent.gameObject) // 自分を除外
                    .OrderBy(a => Vector3.Distance(transform.position, a.transform.position))
                    .FirstOrDefault();
            }
            else // 味方の場合
            {
                EscortTarget = GameObject.FindGameObjectWithTag("Player");
            }
        }

        if (EscortTarget)
        {

            // 移動先設定
            area.destination = EscortTarget.transform.position;
        }
    }
}
