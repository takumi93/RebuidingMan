using UnityEngine;

public class TireLeg : LegBase
{
    // キャタピラ時の角度
    private const float _moveStartAngle = 60.0f;
    private const float _rotateSpeed = 120f;

    public override void CreateSetup()
    {
        UpdateMaterial(LegData);
    }

    public override void Move(Vector3 targetPos)
    {
        _agent.stoppingDistance = _robot.MoveStoppingDistance;

        Vector3 targetDir = targetPos - transform.position;
        targetDir.y = 0f;

        //Vector3 targetDir = _agent.desiredVelocity;
        //targetDir.y = 0;

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
            Debug.DrawLine(transform.position,
               _agent.steeringTarget,
               Color.green);
        }
        else
        {
            _agent.isStopped = true;
        }

        _agent.isStopped = false;
        _agent.SetDestination(targetPos);
    }
}
