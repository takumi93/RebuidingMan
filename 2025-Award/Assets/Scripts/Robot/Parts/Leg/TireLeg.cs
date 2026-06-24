using UnityEngine;

public class TireLeg : LegBase
{
    // キャタピラ時の角度
    private const float _moveStartAngle = 30.0f;
    private const float _rotateSpeed = 120f;

    public override void CreateSetup()
    {
        transform.GetComponentInChildren<SkinnedMeshRenderer>().material = LegData.AllyMaterial;
    }

    public override void Move(Vector3 targetPos)
    {
        Vector3 targetDir = targetPos - transform.position;
        targetDir.y = 0f;

        if (targetDir.sqrMagnitude < 0.01f) return;

        Transform robotRoot = transform.parent;

        // 上下方向は無視して水平だけで追尾 
        // 旋回
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);

        // 回転させたい本体
        robotRoot.rotation = Quaternion.RotateTowards(
                robotRoot.rotation,
                targetRotation,
                _rotateSpeed * Time.deltaTime);

        // 自分の向きとターゲット方向の角度差
        float angle = Vector3.Angle(robotRoot.forward, targetDir);

        if (angle < _moveStartAngle)
        {
            _agent.isStopped = false;
            _agent.destination = targetPos;
        }
        else
        {
            _agent.isStopped = true;
        }
    }
}
